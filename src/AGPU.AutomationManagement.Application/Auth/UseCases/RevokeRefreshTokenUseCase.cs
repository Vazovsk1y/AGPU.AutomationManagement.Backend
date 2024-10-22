using AGPU.AutomationManagement.Application.Auth.Commands;
using AGPU.AutomationManagement.Application.Auth.Services;
using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.DAL.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace AGPU.AutomationManagement.Application.Auth.UseCases;

internal sealed class RevokeRefreshTokenUseCase(
    ICurrentUserProvider currentUserProvider,
    IWriteDbContext writeDbContext) : IUseCase<RevokeRefreshTokenCommand>
{
    public async Task<Result> ExecuteAsync(RevokeRefreshTokenCommand parameter, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var currentUser = await currentUserProvider.GetCurrentUserAsync();
        ArgumentNullException.ThrowIfNull(currentUser);
        
        var refreshToken = await writeDbContext
            .UserTokens
            .SingleOrDefaultAsync(e => e.UserId == currentUser.Id
                                       && e.LoginProvider == RefreshTokenProvider.LoginProvider
                                       && e.Name == RefreshTokenProvider.Name, cancellationToken);

        if (refreshToken is null)
        {
            return Result.Success();
        }
        
        writeDbContext.UserTokens.Remove(refreshToken);
        await writeDbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}