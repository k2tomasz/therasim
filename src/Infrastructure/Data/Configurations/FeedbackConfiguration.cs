using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Therasim.Domain.Entities;

namespace Therasim.Infrastructure.Data.Configurations;

public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.ToTable("Feedbacks");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Content).IsRequired();
        builder.Property(x => x.Message).IsRequired();
        builder.Property(x => x.IsUseful);
        builder.Property(x => x.SessionId).IsRequired();
        builder.HasOne(x => x.Session)
            .WithMany(x => x.Feedbacks)
            .HasForeignKey(x => x.SessionId);

    }
}

