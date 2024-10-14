using AGPU.AutomationManagement.Application.Problem.Commands;
using FluentValidation;

namespace AGPU.AutomationManagement.Application.Validators;

public class ProblemAddCommandValidator : AbstractValidator<ProblemAddCommand>
{
    public ProblemAddCommandValidator()
    {
        // TODO: Использовать ограничения.
        
        RuleFor(e => e.Description)
            .NotEmpty();

        RuleFor(e => e.Audience)
            .NotEmpty();
    }
}