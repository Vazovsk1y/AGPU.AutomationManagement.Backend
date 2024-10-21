using AGPU.AutomationManagement.Application.Problem.Commands;
using AGPU.AutomationManagement.Domain.Constants.Constraints;
using FluentValidation;

namespace AGPU.AutomationManagement.Application.Validators;

internal class ProblemScoreAddCommandValidator : AbstractValidator<ProblemScoreAddCommand>
{
    public ProblemScoreAddCommandValidator()
    {
        RuleFor(e => e.Description)
            .MaximumLength(ScoreConstraints.DescriptionMaxLength);

        RuleFor(e => e.Value)
            .InclusiveBetween(0.25F, 5F);
    }
}