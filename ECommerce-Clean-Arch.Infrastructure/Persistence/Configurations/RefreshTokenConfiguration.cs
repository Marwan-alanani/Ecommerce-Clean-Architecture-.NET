using ECommerce_Clean_Arch.Domain.RefreshTokens;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce_Clean_Arch.Infrastructure.Persistence.Configurations;

public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");
        builder.HasKey(r => r.Id);
        builder.HasIndex(r => r.UserId);
        builder.HasIndex(r => r.TokenHash);
        builder.Property(r => r.TokenHash).HasMaxLength(64).IsRequired();
        builder.Property(r => r.RevokedReason).HasMaxLength(64);
        builder.Property(r => r.UserAgent).HasMaxLength(300);
        builder.Property(r => r.IpAddress).HasMaxLength(200);
        builder.Property(r => r.UserId).IsRequired();
    }
}