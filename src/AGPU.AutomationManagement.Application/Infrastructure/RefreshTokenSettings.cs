namespace AGPU.AutomationManagement.Application.Infrastructure;

public record RefreshTokenSettings
{
    public TimeSpan TokenLifetime { get; init; } = TimeSpan.FromDays(5);
}