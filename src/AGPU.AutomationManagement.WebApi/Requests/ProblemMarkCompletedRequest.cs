namespace AGPU.AutomationManagement.WebApi.Requests;

public record ProblemMarkCompletedRequest(
    string ExecutionDate,
    string ExecutionTime);