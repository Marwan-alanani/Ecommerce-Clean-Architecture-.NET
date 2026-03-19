using ECommerce_Clean_Arch.Domain.Carts;
using ECommerce_Clean_Arch.Domain.Carts.ValueObjects;
using ECommerce_Clean_Arch.Domain.Products.ValueObjects;
using ECommerce_Clean_Arch.Domain.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ECommerce_Clean_Arch.Infrastructure.Persistence.Configurations;

internal sealed class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => CartId.FromValue(value))
            .ValueGeneratedNever();
        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<Cart>(c => c.UserId);
        builder.OwnsMany(
            c => c.Items,
            cib =>
            {
                cib.WithOwner().HasForeignKey("CartId");
                cib.ToTable("CartItems");
                cib.HasKey(ci => ci.Id);
                cib.Property(ci => ci.Id)
                    .HasConversion(
                        id => id.Value,
                        value => CartItemId.FromValue(value)
                    )
                    .ValueGeneratedNever();
                cib.Property(ci => ci.ProductId)
                    .HasConversion(
                        id => id.Value,
                        value => ProductId.FromValue(value)
                    );
                cib.Property(ci => ci.ProductName)
                    .HasMaxLength(100)
                    .IsRequired();
                cib.OwnsOne(
                    ci => ci.UnitPrice,
                    priceBuilder =>
                    {
                        priceBuilder.Property(p => p.Currency)
                            .HasConversion<string>()
                            .HasMaxLength(5)
                            .HasColumnName("Currency");
                        priceBuilder.Property(p => p.Amount)
                            .HasColumnName("Amount")
                            .HasPrecision(18, 2);
                    });
            });
        builder.Metadata
            .FindNavigation(nameof(Cart.Items))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}