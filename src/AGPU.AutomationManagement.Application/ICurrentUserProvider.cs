using AGPU.AutomationManagement.DAL.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace AGPU.AutomationManagement.Application;

internal interface ICurrentUserProvider
{
    Task<Domain.Entities.User?> GetCurrentUserAsync();
}

internal sealed class CurrentUserProvider(IReadDbContext readDbContext) : ICurrentUserProvider
{
    public async Task<Domain.Entities.User?> GetCurrentUserAsync()
    {
        // TODO: Реализовать через httpContextAccessor.
        
        return await readDbContext
            .Users
            .OrderBy(e => Guid.NewGuid())
            .FirstOrDefaultAsync();
    }
} 