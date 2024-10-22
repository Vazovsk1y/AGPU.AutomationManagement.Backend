using AGPU.AutomationManagement.Application.Problem.Commands;
using AGPU.AutomationManagement.Domain.Constants.Constraints;
using FluentValidation;

namespace AGPU.AutomationManagement.Application.Validators;

internal class ProblemAddCommandValidator : AbstractValidator<ProblemAddCommand>
{
    public ProblemAddCommandValidator()
    {
        RuleFor(e => e.Title)
            .NotEmpty()
            .WithMessage("Необходимо указать название.")
            .MaximumLength(ProblemConstraints.TitleMaxLength);
        
        RuleFor(e => e.Description)
            .NotEmpty()
            .WithMessage("Необходимо заполнить описание.")
            .MaximumLength(ProblemConstraints.DescriptionMaxLength);

        RuleFor(e => e.Audience)
            .NotEmpty()
            .WithMessage("Необходимо указать номер аудитории.")
            .MaximumLength(ProblemConstraints.AudienceMaxLength);
    }
}