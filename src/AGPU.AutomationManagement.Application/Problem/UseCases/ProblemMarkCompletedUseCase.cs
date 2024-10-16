using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Extensions;
using AGPU.AutomationManagement.Application.Problem.Commands;
using AGPU.AutomationManagement.DAL.PostgreSQL;
using AGPU.AutomationManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AGPU.AutomationManagement.Application.Problem.UseCases;

internal sealed class ProblemMarkCompletedUseCase(
    IWriteDbContext writeDbContext) : IUseCase<ProblemMarkCompletedCommand>
{
    public async Task<Result> ExecuteAsync(ProblemMarkCompletedCommand parameter, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // TODO: Авторизация.

        var target = await writeDbContext
            .Problems
            .FirstOrDefaultAsync(e => e.Id == parameter.ProblemId, cancellationToken);
        
        var result = await Result
            .SuccessIf(target is not null,
                ($"{nameof(ProblemMarkCompletedUseCase)}.ProblemNotFound", "Проблема не найдена в базе данных."))
            .OnSuccess(() => Result.FailureIf(target!.Status == ProblemStatus.Completed, 
                ($"{nameof(ProblemMarkCompletedUseCase)}.ProblemAlreadyCompleted", "Проблема уже решена.")))
            .OnSuccess(() => Result.FailureIf(target!.Status == ProblemStatus.Pending || target.Status != ProblemStatus.InProgress || target.ContractorId is null, 
                ($"{nameof(ProblemMarkCompletedUseCase)}.ProblemNotInProgress", "Необходимо прикрепить исполнителя.")))
            .OnSuccess(() => Result.FailureIf(target!.ExecutionDateTime < target.CreatedAt, 
                ($"{nameof(ProblemMarkCompletedUseCase)}.InvalidExecutionDateTime", "Отсутствует возможность устанавливать дату и время выполнения задним числом.")))
            .MatchAsync(
                async () =>
                {
                    target!.Status = ProblemStatus.Completed;
                    target.ExecutionDateTime = parameter.ExecutionDateTime;

                    await writeDbContext.SaveChangesAsync(cancellationToken);
                    return Result.Success();
                },
                Result.Failure);

        return result;
    }
}