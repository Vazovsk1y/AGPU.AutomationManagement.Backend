using AGPU.AutomationManagement.Domain.Constants.Constraints;
using AGPU.AutomationManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AGPU.AutomationManagement.DAL.PostgreSQL.Configurations;

public sealed class SolvingScoreConfiguration : IEntityTypeConfiguration<SolvingScore>
{
    public void Configure(EntityTypeBuilder<SolvingScore> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Description)
            .HasMaxLength(SolvingScoreConstraints.DescriptionMaxLength);

        builder.HasOne(e => e.Problem)
            .WithOne(e => e.SolvingScore)
            .HasForeignKey<SolvingScore>(e => e.ProblemId);
    }
}