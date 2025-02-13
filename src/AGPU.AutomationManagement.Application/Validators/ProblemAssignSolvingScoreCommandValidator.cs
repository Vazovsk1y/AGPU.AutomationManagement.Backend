﻿using AGPU.AutomationManagement.Application.Problem.Commands;
using AGPU.AutomationManagement.Domain.Constants.Constraints;
using FluentValidation;

namespace AGPU.AutomationManagement.Application.Validators;

internal class ProblemAssignSolvingScoreCommandValidator : AbstractValidator<ProblemAssignSolvingScoreCommand>
{
    public ProblemAssignSolvingScoreCommandValidator()
    {
        RuleFor(e => e.Description)
            .MaximumLength(SolvingScoreConstraints.DescriptionMaxLength);

        RuleFor(e => e.Value)
            .InclusiveBetween(0.25F, 5F)
            .Must(e => e * 100 % 0.25 == 0)
            .WithMessage("Выставленная оценка должна быть кратна 0.25.");
    }
}