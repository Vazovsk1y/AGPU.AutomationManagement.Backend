using AGPU.AutomationManagement.Application.Auth.Commands;
using FluentValidation;

namespace AGPU.AutomationManagement.Application.Validators;

internal class RefreshTokensCommandValidator : AbstractValidator<RefreshTokensCommand>
{
    public RefreshTokensCommandValidator()
    {
        RuleFor(e => e.RefreshToken)
            .NotEmpty();
    }
}