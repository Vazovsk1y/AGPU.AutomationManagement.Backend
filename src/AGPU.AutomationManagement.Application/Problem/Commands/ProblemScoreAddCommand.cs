namespace AGPU.AutomationManagement.Application.Problem.Commands;

public record ProblemScoreAddCommand(Guid ProblemId, float Value, string? Description);