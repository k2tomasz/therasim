using Therasim.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Therasim.Infrastructure.Data.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.IsActive).IsRequired();
        builder.Property(s => s.SimulationId).IsRequired();
        builder.Property(x => x.FeedbackHistory);
        builder.Property(x => x.ChatHistory);
        builder.HasOne(s => s.Simulation)
            .WithMany(s=>s.Sessions)
            .HasForeignKey(s => s.SimulationId);
    }
}