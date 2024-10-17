using Microsoft.AspNetCore.Identity;

namespace AGPU.AutomationManagement.Application.Infrastructure;

public class ClaimsIdentitySettings : ClaimsIdentityOptions
{
    public required string EmailConfirmedClaimType { get; init; } = "email_confirmed";
}