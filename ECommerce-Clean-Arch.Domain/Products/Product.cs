using ECommerce_Clean_Arch.Domain.Categories.ValueObjects;
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
    public Guid? CreatedBy { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModifiedAt { get; set; }
    public CategoryId? CategoryId { get; set; }


    private Product()
    {
    }

    private Product(
        ProductId id,
        string name,
        string? description,
        Money price,
        string pictureUrl,
        CategoryId? categoryId
    ) : base(id)
    {
        Name = name;
        Description = description;
        Price = price;
        PictureUrl = pictureUrl;
        CategoryId = categoryId;
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
            null);
    }

    public void SetCategoryId(CategoryId? categoryId)
    {
        if (categoryId.HasValue)
        {
            CategoryId = categoryId.Value;
        }
        else CategoryId = null;
    }
}