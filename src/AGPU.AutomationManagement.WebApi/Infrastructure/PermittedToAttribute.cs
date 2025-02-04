using Microsoft.AspNetCore.Authorization;

namespace AGPU.AutomationManagement.WebApi.Infrastructure;

public sealed class PermittedToAttribute : AuthorizeAttribute
{
    public PermittedToAttribute(params string[] roles)
    {
        if (roles.Any(string.IsNullOrWhiteSpace))
        {
            throw new ArgumentException("role");
        }

        Roles = string.Join(',', roles);
    }
}