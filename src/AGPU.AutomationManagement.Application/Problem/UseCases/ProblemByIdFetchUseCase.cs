using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Extensions;
using AGPU.AutomationManagement.Application.Problem.Queries;
using AGPU.AutomationManagement.DAL.PostgreSQL;
using AGPU.AutomationManagement.Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace AGPU.AutomationManagement.Application.Problem.UseCases;

internal sealed class ProblemByIdFetchUseCase(
    ICurrentUserProvider currentUserProvider,
    IReadDbContext readDbContext) : IUseCase<ProblemDTO, ProblemByIdFetchQuery>
{
    public async Task<Result<ProblemDTO>> ExecuteAsync(ProblemByIdFetchQuery parameter, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var currentUser = await currentUserProvider.GetCurrentUserAsync();
        ArgumentNullException.ThrowIfNull(currentUser);

        var problem = await readDbContext
            .Problems
            .Include(e => e.Creator)
            .Include(e => e.Contractor)
            .Include(e => e.SolvingScore)
            .FirstOrDefaultAsync(e => e.Id == parameter.ProblemId, cancellationToken);

        var result = problem
            .EnsureNotNull("Запись не найдена в базе данных.")
            .Ensure(pr => pr.CreatorId == currentUser.Id ||
                          pr.ContractorId == currentUser.Id ||
                          currentUser.Roles.Any(e => e.Role.Name!.Equals(Roles.Administrator, StringComparison.InvariantCultureIgnoreCase)) ||
                          currentUser.Roles.Any(e => e.Role.Name!.Equals(Roles.DeputyAdministrator, StringComparison.InvariantCultureIgnoreCase)),
                "У вас нет полномочий на выполнение данной операции.");

        return result.Match(pr => pr.ToDTO(), Result.Failure<ProblemDTO>);
    }
}