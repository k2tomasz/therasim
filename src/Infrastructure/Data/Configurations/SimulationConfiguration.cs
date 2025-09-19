using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Therasim.Domain.Entities;

namespace Therasim.Infrastructure.Data.Configurations;

public class SimulationConfiguration : IEntityTypeConfiguration<Simulation>
{
    public void Configure(EntityTypeBuilder<Simulation> builder)
    {
        builder.ToTable("Simulations");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.UserId).IsRequired();
        builder.Property(a => a.PersonaId).IsRequired();
        builder.HasOne(a => a.Persona)
            .WithMany(a => a.Simulations)
            .HasForeignKey(a => a.PersonaId);
        builder.Property(a => a.FeedbackType).IsRequired();
        builder.HasMany(a => a.Sessions)
            .WithOne(a => a.Simulation)
            .HasForeignKey(c => c.SimulationId);
    }
}

