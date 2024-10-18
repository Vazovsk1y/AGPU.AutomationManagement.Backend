using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Extensions;
using AGPU.AutomationManagement.Application.User.Queries;
using AGPU.AutomationManagement.DAL.PostgreSQL;
using AGPU.AutomationManagement.Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AGPU.AutomationManagement.Application.User.UseCases;

internal sealed class ContractorsFetchUseCase(
    IReadDbContext readDbContext,
    RoleManager<Domain.Entities.Role> roleManager) : IUseCase<IReadOnlyCollection<ContractorDTO>, ContractorsFetchQuery>
{
    public async Task<Result<IReadOnlyCollection<ContractorDTO>>> ExecuteAsync(ContractorsFetchQuery parameter, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var engineerRole = await roleManager.FindByNameAsync(Roles.Engineer);
        ArgumentNullException.ThrowIfNull(engineerRole);

        var result = await readDbContext
            .Users
            .Include(e => e.Roles)
            .Where(e => e.Roles.Any(i => i.RoleId == engineerRole.Id))
            .Select(e => e.ToContractorDTO())
            .ToListAsync(cancellationToken);

        return result;
    }
}