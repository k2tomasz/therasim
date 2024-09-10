using Therasim.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Therasim.Infrastructure.Data.Configurations;

public class ProblemConfiguration : IEntityTypeConfiguration<Problem>
{
    public void Configure(EntityTypeBuilder<Problem> builder)
    {
        builder.ToTable("Problems");
        builder.HasKey(pp => pp.Id);
        builder.Property(pp => pp.Name).IsRequired().HasMaxLength(100);
        builder.Property(pp => pp.Description).HasMaxLength(500);
    }
}



