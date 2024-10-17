namespace AGPU.AutomationManagement.Application.Infrastructure;

public record AuthSettings
{
    public required TokenSettings Tokens { get; init; }
    
    public required ClaimsIdentitySettings ClaimsIdentity { get; init; }
}