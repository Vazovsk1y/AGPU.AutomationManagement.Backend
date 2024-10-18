using AGPU.AutomationManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AGPU.AutomationManagement.WebApi.Infrastructure;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class ValidateSecurityStampAttribute : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var httpContext = context.HttpContext;
        
        if (httpContext.User.Identity is null or { IsAuthenticated: false })
        {
            return;
        }

        using var scope = httpContext.RequestServices.CreateScope();
        var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<User>>();
        var user = await signInManager.ValidateSecurityStampAsync(httpContext.User);
        if (user is null)
        {
            context.Result = new UnauthorizedResult();
        }
    }
}