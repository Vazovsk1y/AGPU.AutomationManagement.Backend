using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Extensions;
using AGPU.AutomationManagement.Application.Problem.Commands;
using AGPU.AutomationManagement.DAL.PostgreSQL;
using AGPU.AutomationManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AGPU.AutomationManagement.Application.Problem.UseCases;

internal sealed class ProblemAttachContractorUseCase(
    IWriteDbContext writeDbContext) : IUseCase<ProblemAttachContractorCommand>
{
    public async Task<Result> ExecuteAsync(ProblemAttachContractorCommand parameter, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        // TODO: Авторизация.

        var target = await writeDbContext
            .Problems
            .FirstOrDefaultAsync(e => e.Id == parameter.ProblemId, cancellationToken);
        
        var result = await Result
            .SuccessIf(target is not null, 
                ($"{nameof(ProblemAttachContractorUseCase)}.ProblemNotFound", "Проблема не найдена в базе данных."))
            .OnSuccess(() => Result.FailureIf(target!.Status == ProblemStatus.Completed,
                ($"{nameof(ProblemAttachContractorUseCase)}.ProblemAlreadyCompleted", "Проблема уже решена.")))
            .MatchAsync(
                async () =>
                {
                    target!.ContractorId = parameter.ContractorId;
                    target.Status = ProblemStatus.InProgress;

                    await writeDbContext.SaveChangesAsync(cancellationToken);
                    return Result.Success();
                },
                Result.Failure);

        return result;
    }
}