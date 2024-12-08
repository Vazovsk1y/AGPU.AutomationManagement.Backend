using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Extensions;
using AGPU.AutomationManagement.Application.User.Queries;
using AGPU.AutomationManagement.DAL.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace AGPU.AutomationManagement.Application.User.UseCases;

internal sealed class UsersPageFetchUseCase(IReadDbContext readDbContext)
    : IUseCase<PageDTO<UserDTO>, UsersPageFetchQuery>
{
    public async Task<Result<PageDTO<UserDTO>>> ExecuteAsync(UsersPageFetchQuery parameter,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var totalItemsCount = await readDbContext.Users.CountAsync(cancellationToken);

        var result = await readDbContext
            .Users
            .OrderBy(e => e.FullName)
            .ApplyPaging(parameter.PagingOptions)
            .Select(e => new UserDTO(
                e.Id,
                e.FullName,
                e.Email,
                e.UserName,
                e.Post))
            .ToListAsync(cancellationToken);

        return new PageDTO<UserDTO>(result, totalItemsCount, parameter.PagingOptions);
    }
}