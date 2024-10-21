using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Domain.Enums;

namespace AGPU.AutomationManagement.Application.Problem.Queries;

public record ProblemsPageFetchQuery(
    PagingOptions PagingOptions,
    ProblemsPageFilters Filters
    );

public record ProblemsPageFilters(
    ProblemStatus? ByProblemStatus
    );
