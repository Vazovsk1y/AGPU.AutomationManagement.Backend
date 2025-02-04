namespace AGPU.AutomationManagement.Application.Infrastructure.Settings;

public record AuthSettings
{
    public required UserSettings User { get; init; }
    
    public required TokenSettings Tokens { get; init; }
    
    public required ClaimsIdentitySettings ClaimsIdentity { get; init; }
    
    public required PasswordSettings Password { get; init; }
    
    public required LockoutSettings Lockout { get; init; }
    
    public required SignInSettings SignIn { get; init; }
}