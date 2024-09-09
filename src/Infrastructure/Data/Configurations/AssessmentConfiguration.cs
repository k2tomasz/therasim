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
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.Language).IsRequired();
        builder.Property(x => x.StartDate);
        builder.Property(x => x.EndDate);
        builder.Property(x => x.Feedback);
        builder.Property(x => x.ChatHistory);
        builder.Property(a => a.PersonaId).IsRequired();
        builder.HasOne(a => a.Persona)
            .WithMany(a => a.Assessments)
            .HasForeignKey(a => a.PersonaId);
        builder.Property(a => a.SkillId).IsRequired();
        builder.HasOne(a => a.Skill)
            .WithMany(a => a.Assessments)
            .HasForeignKey(a => a.SkillId);
    }
}

