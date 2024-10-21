namespace AGPU.AutomationManagement.Application.Problem.Commands;

public record ProblemAssignSolvingScoreCommand(
    Guid ProblemId, 
    float Value, 
    string? Description);