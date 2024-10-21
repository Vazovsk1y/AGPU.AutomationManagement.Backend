using AGPU.AutomationManagement.Domain.Constants.Constraints;
using AGPU.AutomationManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AGPU.AutomationManagement.DAL.PostgreSQL.Configurations;

public sealed class ScoreConfiguration : IEntityTypeConfiguration<Score>
{
    public void Configure(EntityTypeBuilder<Score> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Description)
            .HasMaxLength(ScoreConstraints.DescriptionMaxLength);

        builder.HasOne(e => e.Problem)
            .WithOne(e => e.Score)
            .HasForeignKey<Score>(e => e.ProblemId);
    }
}