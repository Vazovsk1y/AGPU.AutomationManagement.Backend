using Microsoft.AspNetCore.Identity;

namespace AGPU.AutomationManagement.Application.Infrastructure.Settings;

public class ClaimsIdentitySettings : ClaimsIdentityOptions
{
    public required string EmailConfirmedClaimType { get; init; } = "email_confirmed";
}