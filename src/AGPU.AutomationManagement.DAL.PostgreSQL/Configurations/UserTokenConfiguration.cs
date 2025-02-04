using AGPU.AutomationManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AGPU.AutomationManagement.DAL.PostgreSQL.Configurations;

public sealed class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder
            .HasOne(e => e.User)
            .WithMany(e => e.Tokens)
            .HasForeignKey(e => e.UserId);
    }
}