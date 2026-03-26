using ECommerce_Clean_Arch.Domain.Categories;
using ECommerce_Clean_Arch.Domain.Categories.ValueObjects;
using ECommerce_Clean_Arch.Domain.Products;
using ECommerce_Clean_Arch.Domain.Products.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ECommerce_Clean_Arch.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => ProductId.FromValue(value))
            .ValueGeneratedNever();
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();
        builder.HasIndex(p => p.Name).IsUnique();

        builder.Property(p => p.CategoryId)
            .HasConversion(
                id => id.Value,
                value => CategoryId.FromValue(value)
            );

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(p => p.Description)
            .HasMaxLength(500);


        builder.OwnsOne(
            p => p.Price,
            money =>
            {
                money.Property(p => p.Amount)
                    .HasColumnType("decimal(18,2)")
                    .HasColumnName("Amount")
                    .IsRequired();

                money.Property(p => p.Currency)
                    .HasConversion<string>()
                    .HasMaxLength(5)
                    .HasColumnName("Currency")
                    .IsRequired();
            });

        builder.Property(p => p.PictureUrl)
            .HasMaxLength(500);
        builder.Property(p => p.IsActive);
        builder.Property(p => p.CreatedAt);
        builder.Property(p => p.LastModifiedAt);
    }
}