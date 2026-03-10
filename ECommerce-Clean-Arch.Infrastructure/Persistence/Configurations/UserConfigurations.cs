using ECommerce_Clean_Arch.Domain.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce_Clean_Arch.Infrastructure.Persistence.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(50);
        builder.OwnsMany(
            u => u.RefreshTokens,
            rb =>
            {
                rb.ToTable("RefreshTokens");
                rb.HasKey(r => r.Id);
                rb.Property(r => r.HashedValue).HasMaxLength(200);
                rb.Property(r => r.Id).ValueGeneratedNever();
                rb.WithOwner().HasForeignKey("UserId");
            });

        builder.Metadata
            .FindNavigation(nameof(User.RefreshTokens))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}