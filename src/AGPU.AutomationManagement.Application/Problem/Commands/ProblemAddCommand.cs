using AGPU.AutomationManagement.Domain.Enums;

namespace AGPU.AutomationManagement.Application.Problem.Commands;

public record ProblemAddCommand(
    string Description,
    string Audience,
    ProblemType Type
    );