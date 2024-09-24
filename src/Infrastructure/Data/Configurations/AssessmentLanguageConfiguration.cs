using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Therasim.Domain.Entities;

namespace Therasim.Infrastructure.Data.Configurations
{
    public class AssessmentLanguageConfiguration : IEntityTypeConfiguration<AssessmentLanguage>
    {
        public void Configure(EntityTypeBuilder<AssessmentLanguage> builder)
        {
            builder.HasKey(al => al.Id);

            builder.Property(al => al.Name)
                .IsRequired();

            builder.Property(al => al.Description)
                .IsRequired();

            builder.Property(al => al.Introductions)
                .IsRequired();

            builder.Property(al => al.FeedbackSystemPrompt)
                .IsRequired();

            builder.Property(al => al.Language)
                .IsRequired();

            builder.Property(al => al.AssessmentId)
                .IsRequired();

            builder.HasOne(al => al.Assessment)
                .WithMany(a => a.AssessmentLanguages)
                .HasForeignKey(al => al.AssessmentId);
        }
    }
}
