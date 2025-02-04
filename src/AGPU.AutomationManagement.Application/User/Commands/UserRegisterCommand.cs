namespace AGPU.AutomationManagement.Application.User.Commands;

public record UserRegisterCommand(
    string Username,
    string Password,
    string FullName,
    string Post,
    string Email,
    Guid RoleId
);