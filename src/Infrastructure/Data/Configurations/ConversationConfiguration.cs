using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Therasim.Domain.Entities;

namespace Therasim.Infrastructure.Data.Configurations;

public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.ToTable("Conversations");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.UserId).IsRequired();

        builder.Property(c => c.SimulationId).IsRequired();
        builder.HasOne(c => c.Simulation)
            .WithMany(a => a.Conversations)
            .HasForeignKey(c => c.SimulationId);

        builder.Property(c => c.ChatThreadId).IsRequired();
        builder.Property(c => c.FeedbackThreadId).IsRequired();
    }
}

