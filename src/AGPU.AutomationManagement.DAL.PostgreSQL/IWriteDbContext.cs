namespace AGPU.AutomationManagement.DAL.PostgreSQL;

public interface IWriteDbContext : IReadDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}