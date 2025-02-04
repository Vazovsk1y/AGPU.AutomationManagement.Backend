using AGPU.AutomationManagement.Application.User.Commands;
using AGPU.AutomationManagement.Domain.Constants.Constraints;
using FluentValidation;

namespace AGPU.AutomationManagement.Application.Validators;

internal class UserRegisterCommandValidator : AbstractValidator<UserRegisterCommand>
{
    public UserRegisterCommandValidator()
    {
        RuleFor(e => e.Email)
            .NotEmpty()
            .WithMessage("Необходимо заполнить адрес электронной почты.")
            .EmailAddress();

        RuleFor(e => e.Password)
            .NotEmpty()
            .WithMessage("Необходимо придумать пароль.");

        RuleFor(e => e.Post)
            .NotEmpty()
            .WithMessage("Необходимо указать должность.")
            .MaximumLength(UserConstraints.PostMaxLength);

        RuleFor(e => e.FullName)
            .NotEmpty()
            .WithMessage("Необходимо указать ФИО.")
            .MaximumLength(UserConstraints.FullNameMaxLength);

        RuleFor(e => e.Username)
            .NotEmpty()
            .WithMessage("Необходимо указать логин.");
    }
}