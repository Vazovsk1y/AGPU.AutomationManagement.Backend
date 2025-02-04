namespace AGPU.AutomationManagement.WebApi.Responses;

public record UserResponse(
    Guid Id,
    string FullName,
    string? Email,
    string? Username,
    string Post);