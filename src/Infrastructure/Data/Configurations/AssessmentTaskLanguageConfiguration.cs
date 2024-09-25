using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Therasim.Domain.Entities;

namespace Therasim.Infrastructure.Data.Configurations
{
    public class AssessmentTaskLanguageConfiguration : IEntityTypeConfiguration<AssessmentTaskLanguage>
    {
        public void Configure(EntityTypeBuilder<AssessmentTaskLanguage> builder)
        {
            builder.HasKey(atl => atl.Id);

            builder.Property(atl => atl.Name)
                .IsRequired();

            builder.Property(atl => atl.Instructions)
                .IsRequired();

            builder.Property(atl => atl.Scenario)
                .IsRequired();

            builder.Property(atl => atl.Challenge)
                .IsRequired();

            builder.Property(atl => atl.Skills)
                .IsRequired();

            builder.Property(atl => atl.ClientPersona)
                .IsRequired();

            builder.Property(atl => atl.ClientSystemPrompt)
                .IsRequired();

            builder.Property(atl => atl.FeedbackSystemPrompt)
                .IsRequired();

            builder.Property(atl => atl.Language)
                .IsRequired();

            builder.Property(atl => atl.AssessmentTaskId)
                .IsRequired();

            builder.HasOne(atl => atl.AssessmentTask)
                .WithMany(at => at.AssessmentTaskLanguages)
                .HasForeignKey(atl => atl.AssessmentTaskId);
        }
    }
}

