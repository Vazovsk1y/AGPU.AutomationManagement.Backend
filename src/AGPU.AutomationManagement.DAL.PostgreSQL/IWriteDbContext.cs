using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AGPU.AutomationManagement.DAL.PostgreSQL;

public interface IWriteDbContext : IReadDbContext
{
    DatabaseFacade Database { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}