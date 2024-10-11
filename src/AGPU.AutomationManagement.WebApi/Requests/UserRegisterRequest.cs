namespace AGPU.AutomationManagement.WebApi.Requests;

public record UserRegisterRequest(
    string Username,
    string Password,
    string FullName,
    string Post,
    string Email,
    Guid RoleId);
