﻿using AGPU.AutomationManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AGPU.AutomationManagement.WebApi.Infrastructure;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ValidateEmailConfirmationAttribute : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var httpContext = context.HttpContext;
        
        if (httpContext.User.Identity is null or { IsAuthenticated: false })
        {
            return;
        }

        using var scope = httpContext.RequestServices.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var user = await userManager.GetUserAsync(httpContext.User);
        if (user is null || (userManager.Options.SignIn.RequireConfirmedEmail && !await userManager.IsEmailConfirmedAsync(user)))
        {
            context.Result = new ForbidResult();
        }
    }
}