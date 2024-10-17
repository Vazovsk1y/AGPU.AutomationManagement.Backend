namespace AGPU.AutomationManagement.Application.Infrastructure.Settings;

public record RefreshTokenSettings
{
    public TimeSpan TokenLifetime { get; init; } = TimeSpan.FromDays(5);
}