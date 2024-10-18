namespace AGPU.AutomationManagement.WebApi.Responses;

public record CurrentUserResponse(
    Guid Id,
    string? Username,
    string FullName,
    string Post,
    string? Email,
    bool EmailConfirmed,
    string? PhoneNumber,
    bool PhoneNumberConfirmed
    );