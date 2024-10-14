using System.Reflection;
using AGPU.AutomationManagement.DAL.PostgreSQL.Extensions;
using AGPU.AutomationManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AGPU.AutomationManagement.DAL.PostgreSQL;

public sealed class ApplicationDbContext(DbContextOptions options) : 
    IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>(options), 
    IWriteDbContext
{
    public DbSet<Problem> Problems { get; init; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.Seed();
    }
}