using Therasim.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Therasim.Infrastructure.Data.Configurations;

public class PsychProblemConfiguration : IEntityTypeConfiguration<PsychProblem>
{
    public void Configure(EntityTypeBuilder<PsychProblem> builder)
    {
        builder.ToTable("PsychProblems");

        builder.HasKey(pp => pp.Id);

        builder.Property(pp => pp.Name).IsRequired().HasMaxLength(100);
        builder.Property(pp => pp.Description).HasMaxLength(500);

        builder.HasMany(pp => pp.Simulations)
            .WithOne(s => s.PsychProblem)
            .HasForeignKey(s => s.PsychProblemId);
    }
}



