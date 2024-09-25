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
        builder.HasMany(p => p.AssessmentLanguages)
            .WithOne(a => a.Assessment)
            .HasForeignKey(a => a.AssessmentId);
        builder.HasMany(p => p.UserAssessments)
            .WithOne(a => a.Assessment)
            .HasForeignKey(a => a.AssessmentId);
        builder.HasMany(p => p.AssessmentTasks)
            .WithOne(a => a.Assessment)
            .HasForeignKey(a => a.AssessmentId);
    }
}

