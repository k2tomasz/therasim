using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Therasim.Domain.Entities;

namespace Therasim.Infrastructure.Configurations
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.HasKey(up => up.Id);

            builder.Property(up => up.UserId)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(up => up.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(up => up.Email)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}