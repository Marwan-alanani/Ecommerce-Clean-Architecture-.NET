using ECommerce_Clean_Arch.Domain.Orders;
using ECommerce_Clean_Arch.Domain.Orders.Entities;
using ECommerce_Clean_Arch.Domain.Orders.Enums;
using ECommerce_Clean_Arch.Domain.Orders.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce_Clean_Arch.Infrastructure.Persistence.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.Property(o => o.Id)
            .HasConversion(
                id => id.Value,
                value => OrderId.FromValue(value)
            )
            .ValueGeneratedNever();
        builder.HasKey(o => o.Id);
        builder.Property(o => o.UserId).IsRequired(); // no FK only represents a point in time
        builder.OwnsOne(
            o => o.ShippingAddress,
            addressBuilder =>
            {
                addressBuilder.Property(a => a.Street).HasMaxLength(200).IsRequired();
                addressBuilder.Property(a => a.City).HasMaxLength(100).IsRequired();
                addressBuilder.Property(a => a.Country).HasMaxLength(100).IsRequired();
                addressBuilder.Property(a => a.PostalCode).HasMaxLength(20).IsRequired();
            });
        builder.OwnsMany(o => o.Items, ConfigureOrderItems);
        builder.Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .HasDefaultValue(OrderStatus.Pending);
        builder.Property(o => o.SessionId).HasMaxLength(100);

        builder.Property(o => o.CreatedAt).IsRequired();
        builder.Property(o => o.ConfirmedAt).IsRequired(false);
        builder.Property(o => o.CancelledAt).IsRequired(false);

        builder.Ignore(o => o.Total);
        builder.Ignore(o => o.Currency);
        builder.Metadata
            .FindNavigation(nameof(Order.Items))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private void ConfigureOrderItems(OwnedNavigationBuilder<Order, OrderItem> builder)
    {
        builder.WithOwner().HasForeignKey("OrderId");
        builder.HasKey(nameof(OrderItem.ProductId), "OrderId");
        builder.Property(o => o.ProductName).HasMaxLength(100);
        builder.OwnsOne(
            o => o.UnitPrice,
            unitPriceBuilder =>
            {
                unitPriceBuilder.Property(p => p.Currency)
                    .HasMaxLength(5)
                    .HasColumnName("Currency")
                    .IsRequired();

                unitPriceBuilder.Property(p => p.Amount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Amount")
                    .IsRequired();
            });
        builder.Property(o => o.PictureUrl).HasMaxLength(500);
        builder.Property(o => o.Quantity).IsRequired();
    }
}