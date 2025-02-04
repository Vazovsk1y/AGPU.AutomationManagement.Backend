using AGPU.AutomationManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AGPU.AutomationManagement.DAL.PostgreSQL.Configurations;

public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasMany(e => e.Users)
            .WithOne(e => e.Role)
            .HasForeignKey(ur => ur.RoleId);
    }
}