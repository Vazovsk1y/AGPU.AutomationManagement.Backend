namespace AGPU.AutomationManagement.Application.Auth.Commands;

public record SignInCommand(
    string EmailOrUsername, 
    string Password);