using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Therasim.Domain.Entities;

namespace Therasim.Infrastructure.Data.Configurations
{
    public class UserAssessmentConfiguration : IEntityTypeConfiguration<UserAssessment>
    {
        public void Configure(EntityTypeBuilder<UserAssessment> builder)
        {
            builder.HasKey(ua => ua.Id);

            builder.Property(ua => ua.UserId)
                .IsRequired();

            builder.Property(ua => ua.AssessmentId)
                .IsRequired();

            builder.Property(ua => ua.Feedback);

            builder.HasOne(ua => ua.Assessment)
                .WithMany(a => a.UserAssessments)
                .HasForeignKey(ua => ua.AssessmentId);

            builder.HasMany(ua => ua.UserAssessmentTasks)
                .WithOne(uat => uat.UserAssessment)
                .HasForeignKey(uat => uat.UserAssessmentId);
        }
    }
}
