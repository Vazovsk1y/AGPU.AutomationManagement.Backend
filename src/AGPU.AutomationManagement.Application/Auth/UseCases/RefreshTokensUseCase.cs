using AGPU.AutomationManagement.Application.Auth.Commands;
using AGPU.AutomationManagement.Application.Auth.Services;
using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Infrastructure.Settings;
using AGPU.AutomationManagement.DAL.PostgreSQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AGPU.AutomationManagement.Application.Auth.UseCases;

internal sealed class RefreshTokensUseCase(
    ICurrentUserProvider currentUserProvider,
    UserManager<Domain.Entities.User> userManager,
    IWriteDbContext writeDbContext,
    AuthSettings authSettings,
    TimeProvider timeProvider) : IUseCase<TokensDTO, RefreshTokensCommand>
{
    private readonly TokenSettings _tokenSettings = authSettings.Tokens;
    
    public async Task<Result<TokensDTO>> ExecuteAsync(RefreshTokensCommand parameter, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var currentUser = await currentUserProvider.GetCurrentUserAsync();
        ArgumentNullException.ThrowIfNull(currentUser);
        
        var refreshTokenVerificationResult = await userManager.VerifyUserTokenAsync(
            currentUser, 
            RefreshTokenProvider.LoginProvider,
            RefreshTokenProvider.Name,
            parameter.RefreshToken);

        if (!refreshTokenVerificationResult)
        {
            return Result.Failure<TokensDTO>("Невалидный refresh token.");
        }
        
        var existingRefreshToken = await writeDbContext
            .UserTokens
            .SingleAsync(e => 
                e.UserId == currentUser.Id
                && e.LoginProvider == RefreshTokenProvider.LoginProvider
                && e.Name == RefreshTokenProvider.Name, cancellationToken);

        var newRefreshTokenValue = await userManager.GenerateUserTokenAsync(currentUser, RefreshTokenProvider.LoginProvider, RefreshTokenProvider.Name);
        var newAccessTokenValue = await userManager.GenerateUserTokenAsync(currentUser, AccessTokenProvider.LoginProvider, AccessTokenProvider.Name);

        var currentDateTime = timeProvider.GetUtcNow();
        existingRefreshToken.ExpirationDateTime = currentDateTime.Add(_tokenSettings.Refresh.TokenLifetime);
        existingRefreshToken.Value = newRefreshTokenValue;

        await writeDbContext.SaveChangesAsync(cancellationToken);
        return new TokensDTO(newAccessTokenValue, newRefreshTokenValue);
    }
}