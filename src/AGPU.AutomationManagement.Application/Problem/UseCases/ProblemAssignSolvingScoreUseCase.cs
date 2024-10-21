using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Extensions;
using AGPU.AutomationManagement.Application.Problem.Commands;
using AGPU.AutomationManagement.DAL.PostgreSQL;
using AGPU.AutomationManagement.Domain.Entities;
using AGPU.AutomationManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AGPU.AutomationManagement.Application.Problem.UseCases;

internal sealed class ProblemAssignSolvingScoreUseCase(
    ICurrentUserProvider currentUserProvider,
    IWriteDbContext writeDbContext,
    TimeProvider timeProvider
    ) : IUseCase<ProblemAssignSolvingScoreCommand>
{
    public async Task<Result> ExecuteAsync(ProblemAssignSolvingScoreCommand parameter, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var currentUser = await currentUserProvider.GetCurrentUserAsync();
        ArgumentNullException.ThrowIfNull(currentUser);

        var problem = await writeDbContext
            .Problems
            .FirstOrDefaultAsync(e => e.Id == parameter.ProblemId, cancellationToken);

        var result = await problem
            .EnsureNotNull("Запись не найдена в базе данных.")
            .Ensure(pr => pr.CreatorId == currentUser.Id, "У вас нет полномочий на выполнение данной операции.")
            .Ensure(pr => pr.Status == ProblemStatus.Solved, "Проблема должна быть решена.")
            .MatchAsync(async pr =>
            {
                var score = new SolvingScore
                {
                    CreationDateTime = timeProvider.GetUtcNow(),
                    ProblemId = pr.Id,
                    Value = parameter.Value,
                    Id = Guid.NewGuid(),
                    Description = parameter.Description.TrimIfNotNullOrWhiteSpace(),
                };

                writeDbContext.SolvingScores.Add(score);
                await writeDbContext.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }, Result.Failure);

        return result;
    }
}