using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Domain.Enums;

namespace AGPU.AutomationManagement.Application.Problem.Queries;

public record ProblemOverviewsPageFetchQuery(
    PagingOptions PagingOptions,
    ProblemOverviewsPageFilters Filters
    );

public record ProblemOverviewsPageFilters(
    ProblemStatus? ByProblemStatus);