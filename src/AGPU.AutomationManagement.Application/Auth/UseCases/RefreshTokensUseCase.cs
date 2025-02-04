using AGPU.AutomationManagement.Application.Auth.Commands;
using AGPU.AutomationManagement.Application.Auth.Services;
using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Infrastructure.Settings;
using AGPU.AutomationManagement.DAL.PostgreSQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AGPU.AutomationManagement.Application.Auth.UseCases;

internal sealed class RefreshTokensUseCase(
    UserManager<Domain.Entities.User> userManager,
    IWriteDbContext writeDbContext,
    AuthSettings authSettings,
    TimeProvider timeProvider) : IUseCase<TokensDTO, RefreshTokensCommand>
{
    private readonly TokenSettings _tokenSettings = authSettings.Tokens;
    
    public async Task<Result<TokensDTO>> ExecuteAsync(RefreshTokensCommand parameter, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var target = await writeDbContext
            .UserTokens
            .Include(e => e.User)
            .FirstOrDefaultAsync(e => e.LoginProvider == RefreshTokenProvider.LoginProvider &&
                                      e.Name == RefreshTokenProvider.Name && 
                                      e.Value == parameter.RefreshToken, cancellationToken);

        if (target is null)
        {
            return Result.Failure<TokensDTO>("Целевой refresh токен не найден.");
        }
        
        var newRefreshTokenValue = await userManager.GenerateUserTokenAsync(target.User, RefreshTokenProvider.LoginProvider, RefreshTokenProvider.Name);
        var newAccessTokenValue = await userManager.GenerateUserTokenAsync(target.User, AccessTokenProvider.LoginProvider, AccessTokenProvider.Name);

        var currentDateTime = timeProvider.GetUtcNow();
        target.ExpirationDateTime = currentDateTime.Add(_tokenSettings.Refresh.TokenLifetime);
        target.Value = newRefreshTokenValue;

        await writeDbContext.SaveChangesAsync(cancellationToken);
        return new TokensDTO(newAccessTokenValue, newRefreshTokenValue);
    }
}