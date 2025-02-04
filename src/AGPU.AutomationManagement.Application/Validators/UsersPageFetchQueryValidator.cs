using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.User.Queries;
using FluentValidation;

namespace AGPU.AutomationManagement.Application.Validators;

internal class UsersPageFetchQueryValidator : AbstractValidator<UsersPageFetchQuery>
{
    public UsersPageFetchQueryValidator(IValidator<PagingOptions> pagingOptionsValidator)
    {
        RuleFor(e => e.PagingOptions)
            .SetValidator(pagingOptionsValidator);
    }
}