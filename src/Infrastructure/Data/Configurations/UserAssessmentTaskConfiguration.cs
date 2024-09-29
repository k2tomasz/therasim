using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Therasim.Domain.Entities;

namespace Therasim.Infrastructure.Data.Configurations
{
    public class UserAssessmentTaskConfiguration : IEntityTypeConfiguration<UserAssessmentTask>
    {
        public void Configure(EntityTypeBuilder<UserAssessmentTask> builder)
        {
            builder.HasKey(uat => uat.Id);

            builder.Property(ua => ua.UserId)
                .IsRequired();

            builder.Property(uat => uat.StartDate)
                .IsRequired(false);

            builder.Property(uat => uat.EndDate)
                .IsRequired(false);

            builder.Property(uat => uat.Feedback);

            builder.Property(uat => uat.ChatHistory);

            builder.Property(ua => ua.Language)
                .IsRequired();

            builder.Property(at => at.Order)
                .IsRequired();

            builder.Property(uat => uat.UserAssessmentId)
                .IsRequired();

            builder.Property(uat => uat.AssessmentTaskId)
                .IsRequired();

            builder.HasOne(uat => uat.UserAssessment)
                .WithMany(ua => ua.UserAssessmentTasks)
                .HasForeignKey(uat => uat.UserAssessmentId);

            builder.HasOne(uat => uat.AssessmentTask)
                .WithMany(at => at.UserAssessmentTasks)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(uat => uat.AssessmentTaskId);
        }
    }
}
