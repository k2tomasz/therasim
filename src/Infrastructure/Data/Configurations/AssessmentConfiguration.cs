using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Therasim.Domain.Entities;

namespace Therasim.Infrastructure.Data.Configurations;

public class AssessmentConfiguration : IEntityTypeConfiguration<Assessment>
{
    public void Configure(EntityTypeBuilder<Assessment> builder)
    {
        builder.ToTable("Assessments");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Language).IsRequired();
        builder.Property(x => x.Description).IsRequired().HasMaxLength(500);
        builder.Property(x => x.FeedbackSystemPrompt).IsRequired();
        builder.HasMany(p => p.UserAssessments)
            .WithOne(a => a.Assessment)
            .HasForeignKey(a => a.AssessmentId);
        builder.HasMany(p => p.AssessmentTasks)
            .WithOne(a => a.Assessment)
            .HasForeignKey(a => a.AssessmentId);
    }
}

