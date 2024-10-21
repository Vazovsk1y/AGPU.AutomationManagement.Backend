namespace AGPU.AutomationManagement.WebApi.Responses;

public record ContractorResponse(
    Guid Id,
    string FullName,
    string Post);