using AGPU.AutomationManagement.Application.Auth.Commands;
using AGPU.AutomationManagement.Application.Auth.Services;
using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Extensions;
using AGPU.AutomationManagement.Application.Infrastructure;
using AGPU.AutomationManagement.DAL.PostgreSQL;
using AGPU.AutomationManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AGPU.AutomationManagement.Application.Auth.UseCases;

internal sealed class SignInUseCase(
    UserManager<Domain.Entities.User> userManager,
    IUserConfirmation<Domain.Entities.User> userConfirmation,
    IWriteDbContext writeDbContext,
    AuthSettings authSettings,
    TimeProvider timeProvider) : IUseCase<TokensDTO, SignInCommand>
{
    private readonly SignInOptions _signInOptions = userManager.Options.SignIn;
    private readonly TokenSettings _tokenSettings = authSettings.Tokens;
    
    public async Task<Result<TokensDTO>> ExecuteAsync(SignInCommand parameter, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var canLoginResult = await CanLogin(parameter);

        if (canLoginResult.IsFailure)
        {
            return canLoginResult.ToFailure<TokensDTO>();
        }

        var user = canLoginResult.Value;
        var accessToken = await userManager.GenerateUserTokenAsync(user, AccessTokenProvider.LoginProvider, AccessTokenProvider.Name);
        var refreshToken = await userManager.GenerateUserTokenAsync(user, RefreshTokenProvider.LoginProvider, RefreshTokenProvider.Name);
        
        await ModifyOrCreateRefreshToken(user, refreshToken);
        await userManager.ResetAccessFailedCountAsync(user);
        await writeDbContext.SaveChangesAsync(cancellationToken);
        return new TokensDTO(accessToken, refreshToken);
    }
    
    private async Task<Result<Domain.Entities.User>> CanLogin(SignInCommand command)
    {
        var target = await userManager.FindByEmailAsync(command.EmailOrUsername)
                     ?? await userManager.FindByNameAsync(command.EmailOrUsername);

        var result = await target
            .EnsureNotNull("Неверный логин или пароль.")
            .EnsureAsync(async user => !_signInOptions.RequireConfirmedEmail || await userManager.IsEmailConfirmedAsync(user), 
                "Необходимо подтвердить адрес электронной почты.");
        
        result = await result
            .EnsureAsync(async user => !_signInOptions.RequireConfirmedPhoneNumber || await userManager.IsPhoneNumberConfirmedAsync(user),
                "Необходимо подтвердить номер телефона.");
        
        result = await result
            .EnsureAsync(async user => !_signInOptions.RequireConfirmedAccount || await userConfirmation.IsConfirmedAsync(userManager, user),
                "Необходимо подтвердить аккаунт.");

        result = await result
            .EnsureAsync(async user => !await userManager.IsLockedOutAsync(user),
                "Превышено количество допустимых попыток на вход.");

        return await result
            .MatchAsync(async user =>
                {
                    if (await userManager.CheckPasswordAsync(user, command.Password))
                    {
                        return user;
                    }
                
                    await userManager.AccessFailedAsync(user);
                    return Result.Failure<Domain.Entities.User>("Неверный логин или пароль.");
                }, 
                Result.Failure<Domain.Entities.User>);
    }
    
    private async Task ModifyOrCreateRefreshToken(Domain.Entities.User user, string refreshTokenValue)
    {
        var refreshToken = await writeDbContext
            .UserTokens
            .SingleOrDefaultAsync(e => e.UserId == user.Id
                                       && e.LoginProvider == RefreshTokenProvider.LoginProvider
                                       && e.Name == RefreshTokenProvider.Name);

        var currentDateTime = timeProvider.GetUtcNow();
        if (refreshToken is null)
        {
            refreshToken = new UserToken
            {
                Expires = currentDateTime.Add(_tokenSettings.Refresh.TokenLifetime),
                Value = refreshTokenValue,
                UserId = user.Id,
                LoginProvider = RefreshTokenProvider.LoginProvider,
                Name = RefreshTokenProvider.Name
            };

            writeDbContext.UserTokens.Add(refreshToken);
        }
        else
        {
            refreshToken.Expires = currentDateTime.Add(_tokenSettings.Refresh.TokenLifetime);
            refreshToken.Value = refreshTokenValue;
        }
    }
}