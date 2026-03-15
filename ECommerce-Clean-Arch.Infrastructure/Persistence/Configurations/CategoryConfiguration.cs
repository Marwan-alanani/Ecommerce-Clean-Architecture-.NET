using ECommerce_Clean_Arch.Domain.Categories;
using ECommerce_Clean_Arch.Domain.Categories.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce_Clean_Arch.Infrastructure.Persistence.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => CategoryId.FromValue(value)
            );
        builder.HasKey(c => c.Id);
        builder.HasIndex(c => c.Name).IsUnique();
        builder.Property(c => c.Name)
            .HasMaxLength(50)
            .IsRequired();
    }
}