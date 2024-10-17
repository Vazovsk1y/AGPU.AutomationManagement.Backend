﻿namespace AGPU.AutomationManagement.Application.Infrastructure;

public record AccessTokenSettings
{
    public required string Audience { get; init; }

    public required string Issuer { get; init; }

    public required string SecretKey { get; init; }
    
    public TimeSpan ClockSkew { get; init; } = TimeSpan.FromMinutes(1);

    public TimeSpan TokenLifetime { get; init; } = TimeSpan.FromMinutes(10);
}