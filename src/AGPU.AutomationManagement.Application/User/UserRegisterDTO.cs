namespace AGPU.AutomationManagement.Application.User;

public record UserRegisterDTO(
    string Username,
    string Password,
    string FullName,
    string Post,
    string Email,
    Guid RoleId
);