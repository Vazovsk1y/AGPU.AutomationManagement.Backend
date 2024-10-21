using AGPU.AutomationManagement.Domain.Constants.Constraints;
using AGPU.AutomationManagement.Domain.Entities;
using AGPU.AutomationManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AGPU.AutomationManagement.DAL.PostgreSQL.Configurations;

public sealed class ProblemConfiguration : IEntityTypeConfiguration<Problem>
{
    public void Configure(EntityTypeBuilder<Problem> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Description)
            .HasMaxLength(ProblemConstraints.DescriptionMaxLength);

        builder
            .Property(e => e.Audience)
            .HasMaxLength(ProblemConstraints.AudienceMaxLength);

        builder
            .Property(e => e.Status)
            .HasConversion(e => e.ToString(), e => Enum.Parse<ProblemStatus>(e));

        builder
            .HasOne(e => e.Contractor)
            .WithMany()
            .HasForeignKey(e => e.ContractorId);

        builder
            .Property(e => e.Type)
            .HasConversion(e => e.ToString(), e => Enum.Parse<ProblemType>(e));

        builder
            .HasOne(e => e.Creator)
            .WithMany()
            .HasForeignKey(e => e.CreatorId);

        builder
            .HasOne(e => e.Score)
            .WithOne(e => e.Problem)
            .HasForeignKey<Score>(e => e.ProblemId);
    }
}