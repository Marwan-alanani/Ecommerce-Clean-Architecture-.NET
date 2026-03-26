using ECommerce_Clean_Arch.Domain.Users;
using ECommerce_Clean_Arch.Domain.UserSessions;
using ECommerce_Clean_Arch.Domain.UserSessions.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce_Clean_Arch.Infrastructure.Persistence.Configurations;

public sealed class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.ToTable("UserSessions");
        builder.Property(r => r.Id)
            .HasConversion(
                id => id.Value,
                value => UserSessionId.FromValue(value)
            );
        builder.HasKey(r => r.Id);
        builder.HasIndex(r => r.UserId);
        builder.Property(r => r.RevokedReason)
            .HasConversion<string>()
            .HasMaxLength(64);

        builder.Property(r => r.UserAgent).HasMaxLength(300);
        builder.Property(r => r.IpAddress).HasMaxLength(200);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}