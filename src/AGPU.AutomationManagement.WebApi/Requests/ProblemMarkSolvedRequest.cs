namespace AGPU.AutomationManagement.WebApi.Requests;

public record ProblemMarkSolvedRequest(
    string SolvingDate,
    string SolvingTime);