using ECommerce_Clean_Arch.Domain.Categories.ValueObjects;
using ECommerce_Clean_Arch.Domain.Common.Models;

namespace ECommerce_Clean_Arch.Domain.Categories;

public sealed class Category : AggregateRoot<CategoryId>
{
    private Category()
    {
    }


    public string Name { get; private set; } = null!;

    public static Category Create(string name)
    {
        return new() { Id = CategoryId.CreateUnique(), Name = name };
    }
}