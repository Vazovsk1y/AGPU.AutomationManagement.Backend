using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AGPU.AutomationManagement.DAL.PostgreSQL;

internal sealed class ApplicationDesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    private const string ConnectionString = "User ID=postgres;Password=12345678;Host=localhost;Port=5432;Database=AGPUdb;";
    
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseNpgsql(ConnectionString);
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}