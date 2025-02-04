using AGPU.AutomationManagement.Application.Auth.Commands;
using AGPU.AutomationManagement.Application.Auth.Services;
using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Extensions;
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

        var currentUser = await currentUserProvider.GetRequiredCurrentUserAsync();
        
        await writeDbContext
            .UserTokens
            .Where(e => e.UserId == currentUser.Id &&
                        e.LoginProvider == RefreshTokenProvider.LoginProvider &&
                        e.Name == RefreshTokenProvider.Name)
            .ExecuteDeleteAsync(cancellationToken);

        return Result.Success();
    }
}