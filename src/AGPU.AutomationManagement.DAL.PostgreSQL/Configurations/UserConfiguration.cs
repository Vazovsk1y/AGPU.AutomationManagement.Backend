using AGPU.AutomationManagement.Domain.Constants.Constraints;
using AGPU.AutomationManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AGPU.AutomationManagement.DAL.PostgreSQL.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .Property(e => e.FullName)
            .HasMaxLength(UserConstraints.FullNameMaxLength);

        builder
            .Property(e => e.Post)
            .HasMaxLength(UserConstraints.PostMaxLength);
        
        builder
            .HasMany(e => e.Roles)
            .WithOne(e => e.User)
            .HasForeignKey(u => u.UserId);

        builder
            .HasIndex(e => e.NormalizedEmail)
            .IsUnique();
    }
}