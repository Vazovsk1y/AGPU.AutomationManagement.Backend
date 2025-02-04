namespace AGPU.AutomationManagement.WebApi.Requests;

public record SignInRequest(
    string EmailOrUsername,
    string Password);