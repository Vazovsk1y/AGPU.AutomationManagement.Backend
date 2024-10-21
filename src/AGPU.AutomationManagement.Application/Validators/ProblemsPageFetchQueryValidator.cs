using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Problem.Queries;
using FluentValidation;

namespace AGPU.AutomationManagement.Application.Validators;

internal class ProblemsPageFetchQueryValidator : AbstractValidator<ProblemsPageFetchQuery>
{
    public ProblemsPageFetchQueryValidator(IValidator<PagingOptions> pagingOptionsValidator)
    {
        RuleFor(e => e.PagingOptions)
            .SetValidator(pagingOptionsValidator);

        RuleFor(e => e.Filters)
            .NotNull();
    }
}