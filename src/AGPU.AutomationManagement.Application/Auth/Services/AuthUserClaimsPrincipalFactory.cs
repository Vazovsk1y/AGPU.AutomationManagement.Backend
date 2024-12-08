using System.Security.Claims;
using AGPU.AutomationManagement.Application.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace AGPU.AutomationManagement.Application.Auth.Services;

internal sealed class AuthUserClaimsPrincipalFactory(
    UserManager<Domain.Entities.User> userManager,
    RoleManager<Domain.Entities.Role> roleManager,
    IOptions<IdentityOptions> identityOptions,
    AuthSettings authSettings)
    : UserClaimsPrincipalFactory<Domain.Entities.User, Domain.Entities.Role>(userManager, roleManager, identityOptions)
{
    private readonly ClaimsIdentitySettings _claimsIdentity = authSettings.ClaimsIdentity;
    
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(Domain.Entities.User user)
    {
        var baseCi = await base.GenerateClaimsAsync(user);
        baseCi.AddClaim(new Claim(_claimsIdentity.EmailConfirmedClaimType, user.EmailConfirmed.ToString()));

        return new ClaimsIdentity(baseCi.Claims, JwtBearerDefaults.AuthenticationScheme);
    }
}