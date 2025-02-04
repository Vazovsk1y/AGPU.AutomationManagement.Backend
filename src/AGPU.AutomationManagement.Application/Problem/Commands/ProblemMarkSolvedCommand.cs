namespace AGPU.AutomationManagement.Application.Problem.Commands;

public record ProblemMarkSolvedCommand(
    Guid ProblemId,
    DateTimeOffset SolvingDateTime);
