namespace AGPU.AutomationManagement.Application.Problem.Commands;

public record ProblemMarkCompletedCommand(
    Guid ProblemId,
    DateTimeOffset ExecutionDateTime);
