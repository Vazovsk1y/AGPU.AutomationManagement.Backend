namespace AGPU.AutomationManagement.WebApi.Requests;

public record ProblemAttachContractorRequest(
    Guid ProblemId,
    Guid ContractorId);