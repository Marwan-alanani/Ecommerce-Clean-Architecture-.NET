using ECommerce_Clean_Arch.Domain.Common.Interfaces;

namespace ECommerce_Clean_Arch.Domain.Categories.ValueObjects;

public readonly record struct CategoryId : IAggregateRootId<CategoryId>
{
    public Guid Value { get; }

    private CategoryId(Guid value)
    {
        Value = value;
    }

    public static CategoryId FromValue(Guid value)
    {
        return new CategoryId(value);
    }

    public static CategoryId CreateUnique()
    {
        return new(Guid.NewGuid());
    }
}