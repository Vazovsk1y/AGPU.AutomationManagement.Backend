namespace AGPU.AutomationManagement.Application.Problem.Commands;

public record ProblemAttachContractorCommand(
    Guid ProblemId,
    Guid ContractorId);