using AGPU.AutomationManagement.Application.User.Commands;
using FluentValidation;

namespace AGPU.AutomationManagement.Application.Validators;

public class UserRegisterCommandValidator : AbstractValidator<UserRegisterCommand>
{
    public UserRegisterCommandValidator()
    {
        RuleFor(e => e.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(e => e.Password)
            .NotEmpty();

        RuleFor(e => e.Post)
            .NotEmpty();

        RuleFor(e => e.FullName)
            .NotEmpty();

        RuleFor(e => e.Username)
            .NotEmpty();
    }
}