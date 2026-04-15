using ECommerce_Clean_Arch.Domain.ProductComments;
using ECommerce_Clean_Arch.Domain.ProductComments.ValueObjects;
using ECommerce_Clean_Arch.Domain.Products;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce_Clean_Arch.Infrastructure.Persistence.Configurations;

public sealed class ProductCommentConfiguration : IEntityTypeConfiguration<ProductComment>
{
    public void Configure(EntityTypeBuilder<ProductComment> builder)
    {
        builder.ToTable("ProductComments");
        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => ProductCommentId.FromValue(value)
            );
        builder.HasKey(p => p.Id);
        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Property(p => p.UserName).HasMaxLength(50);
        builder.Property(p => p.Content).HasMaxLength(250);
    }
}