using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Therasim.Domain.Entities;

namespace Therasim.Infrastructure.Data.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Messages");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Content).IsRequired();
        builder.Property(m => m.Role).IsRequired();
        builder.Property(m => m.SessionId).IsRequired();
        builder.HasOne(m => m.Session)
            .WithMany(m=>m.Messages)
            .HasForeignKey(m => m.SessionId);
    }
}

