using AGPU.AutomationManagement.Application.Common;
using FluentValidation;

namespace AGPU.AutomationManagement.Application.Validators;

internal class PagingOptionsValidator : AbstractValidator<PagingOptions>
{
    public PagingOptionsValidator()
    {
        RuleFor(e => e.PageSize)
            .GreaterThanOrEqualTo(1);

        RuleFor(e => e.PageIndex)
            .GreaterThanOrEqualTo(1);
    }
}