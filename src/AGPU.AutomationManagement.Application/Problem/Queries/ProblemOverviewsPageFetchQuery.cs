using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Domain.Enums;

namespace AGPU.AutomationManagement.Application.Problem.Queries;

public record ProblemOverviewsPageFetchQuery(
    PagingOptions PagingOptions,
    ProblemsPageFilters Filters
    );

public record ProblemsPageFilters(
    ProblemStatus? ByProblemStatus
    );
