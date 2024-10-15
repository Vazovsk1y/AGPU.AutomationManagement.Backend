using AGPU.AutomationManagement.Application.Common;
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

        if (target is null)
        {
            return Result.Failure(($"{nameof(ProblemAttachContractorUseCase)}.ProblemNotFound", "Проблема не найдена в базе данных."));
        }
        
        if (target.Status == ProblemStatus.Completed)
        {
            return Result.Failure(($"{nameof(ProblemAttachContractorUseCase)}.ProblemAlreadyCompleted", "Проблема уже решена."));
        }

        target.ContractorId = parameter.ContractorId;
        target.Status = ProblemStatus.InProgress;

        await writeDbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}