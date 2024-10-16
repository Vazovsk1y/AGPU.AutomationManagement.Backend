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

        var result = await target
            .EnsureNotNull("Запись не найдена в базе данных.")
            .Ensure(problem => problem.Status != ProblemStatus.Completed, "Проблема уже решена.")
            .MatchAsync(
                async problem =>
                {
                    problem.ContractorId = parameter.ContractorId;
                    problem.Status = ProblemStatus.InProgress;
                    
                    await writeDbContext.SaveChangesAsync(cancellationToken);
                    return Result.Success();
                },
                Result.Failure
            );
        
        return result;
    }
}