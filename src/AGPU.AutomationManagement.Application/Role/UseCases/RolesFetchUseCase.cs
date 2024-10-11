using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Role.Queries;
using AGPU.AutomationManagement.DAL.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace AGPU.AutomationManagement.Application.Role.UseCases;

internal sealed class RolesFetchUseCase(IReadDbContext readDbContext) : IUseCase<IReadOnlyCollection<RoleDTO>, RolesFetchQuery>
{
    public async Task<Result<IReadOnlyCollection<RoleDTO>>> HandleAsync(RolesFetchQuery parameter, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        // TODO: Проверка ролей.
        
        var result = await readDbContext
            .Roles
            .Select(e => new RoleDTO(e.Id, e.Name!))
            .ToListAsync(cancellationToken);

        return result;
    }
}