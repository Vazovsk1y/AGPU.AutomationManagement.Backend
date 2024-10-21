using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Extensions;
using AGPU.AutomationManagement.Application.Problem.Commands;
using AGPU.AutomationManagement.DAL.PostgreSQL;
using AGPU.AutomationManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AGPU.AutomationManagement.Application.Problem.UseCases;

internal sealed class ProblemMarkSolvedUseCase(
    IWriteDbContext writeDbContext,
    ICurrentUserProvider currentUserProvider) : IUseCase<ProblemMarkSolvedCommand>
{
    public async Task<Result> ExecuteAsync(ProblemMarkSolvedCommand parameter, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var currentUser = await currentUserProvider.GetCurrentUserAsync();
        ArgumentNullException.ThrowIfNull(currentUser);

        var target = await writeDbContext
            .Problems
            .FirstOrDefaultAsync(e => e.Id == parameter.ProblemId, cancellationToken);
        
        var result = await target
            .EnsureNotNull("Запись не найдена в базе данных.")
            .Ensure(pr => pr.Status == ProblemStatus.InProgress, "Невозможно выполнить данное действие.")
            .Ensure(pr => parameter.SolvingDateTime > pr.CreationDateTime, "Дата и время решения не могут быть раньше даты и времени создания проблемы.")
            .Ensure(pr => pr.ContractorId == currentUser.Id, "У вас нет полномочий на выполнение данной операции.")
            .MatchAsync(
                async pr =>
                {
                    pr.Status = ProblemStatus.Solved;
                    pr.SolvingDateTime = parameter.SolvingDateTime;

                    await writeDbContext.SaveChangesAsync(cancellationToken);
                    return Result.Success();
                },
                Result.Failure);
        
        return result;
    }
}