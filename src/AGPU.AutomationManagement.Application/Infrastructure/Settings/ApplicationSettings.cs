namespace AGPU.AutomationManagement.Application.Infrastructure.Settings;

public record ApplicationSettings
{
    public required AuthSettings AuthSettings { get; init; }
    
    public required string DataProtectionKeysPath { get; init; }
}