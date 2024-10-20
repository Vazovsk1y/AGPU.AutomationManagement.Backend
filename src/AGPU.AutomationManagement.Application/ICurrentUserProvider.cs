using System.Security.Claims;
using AGPU.AutomationManagement.Application.Infrastructure.Settings;
using AGPU.AutomationManagement.DAL.PostgreSQL;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AGPU.AutomationManagement.Application;

public interface ICurrentUserProvider
{
    Task<Domain.Entities.User?> GetCurrentUserAsync();
}

internal sealed class CurrentUserProvider(
    AuthSettings authSettings,
    IReadDbContext readDbContext,
    IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
{
    private readonly ClaimsIdentitySettings _claimsIdentitySettings = authSettings.ClaimsIdentity;
    private readonly HttpContext _currentHttpContext = httpContextAccessor.HttpContext ?? throw new InvalidOperationException();
    
    public async Task<Domain.Entities.User?> GetCurrentUserAsync()
    {
        var claimsPrincipal = _currentHttpContext.User;
        if (claimsPrincipal.Identity is null or { IsAuthenticated: false })
        {
            return null;
        }

        _ = Guid.TryParse(claimsPrincipal.FindFirstValue(_claimsIdentitySettings.UserIdClaimType), out var userId);
        return await readDbContext
            .Users
            .Include(e => e.Roles)
            .ThenInclude(e => e.Role)
            .FirstOrDefaultAsync(e => e.Id == userId);
    }
} 