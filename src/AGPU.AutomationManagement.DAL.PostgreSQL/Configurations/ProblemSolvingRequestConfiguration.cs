using AGPU.AutomationManagement.Domain.Constants.Constraints;
using AGPU.AutomationManagement.Domain.Entities;
using AGPU.AutomationManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AGPU.AutomationManagement.DAL.PostgreSQL.Configurations;

public sealed class ProblemSolvingRequestConfiguration : IEntityTypeConfiguration<ProblemSolvingRequest>
{
    public void Configure(EntityTypeBuilder<ProblemSolvingRequest> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Description)
            .HasMaxLength(ProblemSolvingRequestConstraints.DescriptionMaxLength);

        builder
            .Property(e => e.Audience)
            .HasMaxLength(ProblemSolvingRequestConstraints.AudienceMaxLength);

        builder
            .Property(e => e.Status)
            .HasConversion(e => e.ToString(), e => Enum.Parse<ProblemSolvingRequestStatus>(e));

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
    }
}