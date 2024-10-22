using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Extensions;
using AGPU.AutomationManagement.Application.Problem.Queries;
using AGPU.AutomationManagement.DAL.PostgreSQL;
using AGPU.AutomationManagement.Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace AGPU.AutomationManagement.Application.Problem.UseCases;

internal sealed class ProblemOverviewsPageFetchUseCase(
    IReadDbContext readDbContext,
    ICurrentUserProvider currentUserProvider) : IUseCase<PageDTO<ProblemOverviewDTO>, ProblemOverviewsPageFetchQuery>
{
    public async Task<Result<PageDTO<ProblemOverviewDTO>>> ExecuteAsync(ProblemOverviewsPageFetchQuery parameter, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var currentUser = await currentUserProvider.GetCurrentUserAsync();
        ArgumentNullException.ThrowIfNull(currentUser);
        
        var totalItemsCountQuery = readDbContext
            .Problems
            .AsQueryable();

        totalItemsCountQuery = ApplyFiltering(totalItemsCountQuery, currentUser, parameter.Filters);
        var totalItemsCount = await totalItemsCountQuery.CountAsync(cancellationToken);

        var resultQuery = readDbContext
            .Problems
            .Include(e => e.Creator)
            .Include(e => e.Contractor)
            .Include(e => e.SolvingScore)
            .AsQueryable();

        resultQuery = ApplyFiltering(resultQuery, currentUser, parameter.Filters);
        
        var result = await resultQuery
            .OrderByDescending(e => e.CreationDateTime)
            .ApplyPaging(parameter.PagingOptions)
            .Select(e => e.ToPreviewDTO())
            .ToListAsync(cancellationToken);

        return new PageDTO<ProblemOverviewDTO>(result, totalItemsCount, parameter.PagingOptions);
    }

    private static IQueryable<Domain.Entities.Problem> ApplyFiltering(IQueryable<Domain.Entities.Problem> source, Domain.Entities.User currentUser, ProblemsPageFilters filters)
    {
        var result = currentUser.Roles switch
        {
            { Count: 1 } when currentUser.Roles[0].Role.Name!.Equals(Roles.User, StringComparison.InvariantCultureIgnoreCase) =>
                source.Where(e => e.CreatorId == currentUser.Id),
            { Count: 1 } when currentUser.Roles[0].Role.Name!.Equals(Roles.Engineer, StringComparison.InvariantCultureIgnoreCase) =>
                source.Where(e => e.ContractorId == currentUser.Id),
            _ => source
        };

        if (filters.ByProblemStatus is not null)
        {
            result = result.Where(e => e.Status == filters.ByProblemStatus);
        }

        return result;
    }
}