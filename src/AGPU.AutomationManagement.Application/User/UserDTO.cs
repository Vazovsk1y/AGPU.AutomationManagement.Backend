namespace AGPU.AutomationManagement.Application.User;

public record UserDTO(
    Guid Id,
    string FullName,
    string? Email,
    string? Username,
    string Post);