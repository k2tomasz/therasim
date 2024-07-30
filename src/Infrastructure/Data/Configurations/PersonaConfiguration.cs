using Therasim.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Therasim.Infrastructure.Data.Configurations;

public class PersonaConfiguration : IEntityTypeConfiguration<Persona>
{
    public void Configure(EntityTypeBuilder<Persona> builder)
    {
        builder.ToTable("Personas");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Background).HasMaxLength(500);
        builder.HasMany(p => p.Simulations)
            .WithOne(a => a.Persona)
            .HasForeignKey(a => a.PersonaId);
    }
}

