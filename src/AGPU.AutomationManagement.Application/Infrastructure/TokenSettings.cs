namespace AGPU.AutomationManagement.Application.Infrastructure;

public record TokenSettings
{
    public required RefreshTokenSettings Refresh { get; init; }
    
    public required AccessTokenSettings Access { get; init; }
}