using ECommerce_Clean_Arch.Domain.Common.Interfaces;
using ECommerce_Clean_Arch.Domain.Common.Models;
using ECommerce_Clean_Arch.Domain.Products.ValueObjects;
using SharedKernel.Models;

namespace ECommerce_Clean_Arch.Domain.Products;

public class Product : AggregateRoot<ProductId>, IAuditable
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public Money Price { get; set; }
    public string PictureUrl { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; }


    private Product()
    {
    }

    private Product(
        ProductId id,
        string name,
        string? description,
        Money price,
        string pictureUrl,
        bool isActive
    ) : base(id)
    {
        Name = name;
        Description = description;
        Price = price;
        PictureUrl = pictureUrl;
        IsActive = isActive;
    }


    public static Product Create(
        string name,
        string description,
        Money price,
        string pictureUrl
    )
    {
        return new(
            ProductId.CreateUnique(),
            name,
            description,
            price,
            pictureUrl,
            true);
    }

    public void Deactivate() => IsActive = false;
}