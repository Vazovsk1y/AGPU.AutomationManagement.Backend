using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Extensions;
using AGPU.AutomationManagement.Application.Problem.Queries;
using AGPU.AutomationManagement.DAL.PostgreSQL;
using AGPU.AutomationManagement.Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AGPU.AutomationManagement.Application.Problem.UseCases;

internal sealed class ProblemsPageFetchUseCase(
    IReadDbContext readDbContext,
    UserManager<Domain.Entities.User> userManager,
    ICurrentUserProvider currentUserProvider) : IUseCase<PageDTO<ProblemDTO>, ProblemsPageFetchQuery>
{
    public async Task<Result<PageDTO<ProblemDTO>>> ExecuteAsync(ProblemsPageFetchQuery parameter, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var currentUser = await currentUserProvider.GetCurrentUserAsync();
        ArgumentNullException.ThrowIfNull(currentUser);
        
        var totalItemsCountQuery = readDbContext
            .Problems
            .AsQueryable();

        totalItemsCountQuery = ApplyFiltering(totalItemsCountQuery, currentUser);
        var totalItemsCount = await totalItemsCountQuery.CountAsync(cancellationToken);

        var resultQuery = readDbContext
            .Problems
            .Include(e => e.Creator)
            .Include(e => e.Contractor)
            .AsQueryable();

        resultQuery = ApplyFiltering(resultQuery, currentUser);
        
        // TODO: Сортировка или фильтрация по статусу.
        
        var result = await resultQuery
            .OrderByDescending(e => e.CreatedAt)
            .ApplyPaging(parameter.PagingOptions)
            .Select(e => e.ToDTO())
            .ToListAsync(cancellationToken);

        return new PageDTO<ProblemDTO>(result, totalItemsCount, parameter.PagingOptions);
    }

    private static IQueryable<Domain.Entities.Problem> ApplyFiltering(IQueryable<Domain.Entities.Problem> source, Domain.Entities.User currentUser)
    {
        return currentUser.Roles switch
        {
            { Count: 1 } when currentUser.Roles[0].Role.Name!.Equals(Roles.User, StringComparison.InvariantCultureIgnoreCase) =>
                source.Where(e => e.CreatorId == currentUser.Id),
            { Count: 1 } when currentUser.Roles[0].Role.Name!.Equals(Roles.Engineer, StringComparison.InvariantCultureIgnoreCase) =>
                source.Where(e => e.ContractorId == currentUser.Id),
            _ => source
        };
    }
}