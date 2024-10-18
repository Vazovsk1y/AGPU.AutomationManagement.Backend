using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace AGPU.AutomationManagement.Application;

internal interface ICurrentUserProvider
{
    Task<Domain.Entities.User?> GetCurrentUserAsync();
}

internal sealed class CurrentUserProvider(
    UserManager<Domain.Entities.User> userManager,
    IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
{
    private readonly HttpContext _currentHttpContext = httpContextAccessor.HttpContext ?? throw new InvalidOperationException();
    
    public async Task<Domain.Entities.User?> GetCurrentUserAsync()
    {
        var claimsPrincipal = _currentHttpContext.User;
        if (claimsPrincipal.Identity is null or { IsAuthenticated: false })
        {
            return null;
        }

        return await userManager.GetUserAsync(claimsPrincipal);
    }
} 