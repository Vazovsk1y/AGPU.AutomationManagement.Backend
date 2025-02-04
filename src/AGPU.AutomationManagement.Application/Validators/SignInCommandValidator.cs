using AGPU.AutomationManagement.Application.Auth.Commands;
using FluentValidation;

namespace AGPU.AutomationManagement.Application.Validators;

internal class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(e => e.EmailOrUsername)
            .NotEmpty();

        RuleFor(e => e.Password)
            .NotEmpty();
    }
}