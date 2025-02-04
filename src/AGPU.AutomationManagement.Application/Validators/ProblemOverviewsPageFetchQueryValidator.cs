using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Problem.Queries;
using FluentValidation;

namespace AGPU.AutomationManagement.Application.Validators;

internal class ProblemOverviewsPageFetchQueryValidator : AbstractValidator<ProblemOverviewsPageFetchQuery>
{
    public ProblemOverviewsPageFetchQueryValidator(IValidator<PagingOptions> pagingOptionsValidator)
    {
        RuleFor(e => e.PagingOptions)
            .SetValidator(pagingOptionsValidator);

        RuleFor(e => e.Filters)
            .NotNull();
    }
}