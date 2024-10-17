namespace AGPU.AutomationManagement.Application.Infrastructure.Settings;

public record TokenSettings
{
    public required RefreshTokenSettings Refresh { get; init; }
    
    public required AccessTokenSettings Access { get; init; }
}