using AGPU.AutomationManagement.Domain.Enums;

namespace AGPU.AutomationManagement.Application.Problem.Commands;

public record ProblemAddCommand(
    string Title,
    string Description,
    string Audience,
    ProblemType Type);