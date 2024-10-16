﻿using AGPU.AutomationManagement.Application.Common;
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
        
        var result = await target
            .EnsureNotNull("Запись не найдена в базе данных.")
            .Ensure(pr => pr.Status == ProblemStatus.InProgress, "Невозможно выполнить данное действие.")
            .Ensure(pr => parameter.ExecutionDateTime > pr.CreatedAt, "Дата и время выполнения не могут быть раньше даты и времени создания проблемы.")
            .MatchAsync(
                async pr =>
                {
                    pr.Status = ProblemStatus.Completed;
                    pr.ExecutionDateTime = parameter.ExecutionDateTime;

                    await writeDbContext.SaveChangesAsync(cancellationToken);
                    return Result.Success();
                },
                Result.Failure);
        
        return result;
    }
}