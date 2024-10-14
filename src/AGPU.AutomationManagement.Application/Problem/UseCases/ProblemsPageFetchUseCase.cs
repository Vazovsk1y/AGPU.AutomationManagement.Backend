using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Extensions;
using AGPU.AutomationManagement.Application.Problem.Queries;
using AGPU.AutomationManagement.DAL.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace AGPU.AutomationManagement.Application.Problem.UseCases;

internal sealed class ProblemsPageFetchUseCase(
    IReadDbContext readDbContext) : IUseCase<PageDTO<ProblemDTO>, ProblemsPageFetchQuery>
{
    public async Task<Result<PageDTO<ProblemDTO>>> ExecuteAsync(ProblemsPageFetchQuery parameter, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        // TODO: Авторизация.
        
        // TODO: Разные данные и сортировка в зависимости от роли текущего пользователя.

        var totalItemsCount = await readDbContext.Problems.CountAsync(cancellationToken);

        var result = await readDbContext
            .Problems
            .Include(e => e.Creator)
            .Include(e => e.Contractor)
            .OrderBy(e => e.CreatedAt)
            .ApplyPaging(parameter.PagingOptions)
            .Select(e => e.ToDTO())
            .ToListAsync(cancellationToken);

        return new PageDTO<ProblemDTO>(result, totalItemsCount, parameter.PagingOptions);
    }
}