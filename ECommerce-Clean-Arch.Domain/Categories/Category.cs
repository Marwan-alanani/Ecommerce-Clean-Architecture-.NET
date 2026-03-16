using ECommerce_Clean_Arch.Domain.Categories.ValueObjects;
using ECommerce_Clean_Arch.Domain.Common.Interfaces;
using ECommerce_Clean_Arch.Domain.Common.Models;


namespace ECommerce_Clean_Arch.Domain.Categories;

public sealed class Category : AggregateRoot<CategoryId>, IAuditable
{
    private Category()
    {
    }

    public Guid? CreatedBy { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModifiedAt { get; set; }

    public string Name { get; set; } = null!;

    public static Category Create(string name)
    {
        return new() { Id = CategoryId.CreateUnique(), Name = name };
    }
}