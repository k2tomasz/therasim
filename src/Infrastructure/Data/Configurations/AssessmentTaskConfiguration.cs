using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Therasim.Domain.Entities;

namespace Therasim.Infrastructure.Data.Configurations;

public class AssessmentTaskConfiguration : IEntityTypeConfiguration<AssessmentTask>
{
    public void Configure(EntityTypeBuilder<AssessmentTask> builder)
    {
        builder.HasKey(at => at.Id);

        builder.Property(at => at.Objective)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(at => at.ClientSystemPrompt)
            .IsRequired();

        builder.Property(at => at.FeedbackSystemPrompt)
            .IsRequired();

        builder.Property(at => at.Language)
            .IsRequired();

        builder.Property(at => at.LengthInMinutes)
            .IsRequired(false);

        builder.Property(at => at.LengthInInteractionCycles)
            .IsRequired(false);

        builder.Property(at => at.AssessmentId)
            .IsRequired();

        builder.HasOne(at => at.Assessment)
            .WithMany(a => a.AssessmentTasks)
            .HasForeignKey(at => at.AssessmentId);

        builder.HasMany(at => at.UserAssessmentTasks)
            .WithOne(uat => uat.AssessmentTask)
            .HasForeignKey(uat => uat.AssessmentTaskId);
    }
}