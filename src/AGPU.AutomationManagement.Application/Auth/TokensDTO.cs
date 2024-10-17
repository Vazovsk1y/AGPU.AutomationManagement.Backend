namespace AGPU.AutomationManagement.Application.Auth;

public record TokensDTO(
    string AccessToken, 
    string RefreshToken);