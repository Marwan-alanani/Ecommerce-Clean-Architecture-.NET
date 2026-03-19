using ECommerce_Clean_Arch.Domain.Common.Interfaces;

namespace ECommerce_Clean_Arch.Domain.Carts.ValueObjects;

public readonly record struct CartId : IAggregateRootId<CartId>
{
    public Guid Value { get; }

    public CartId() { }

    private CartId(Guid value)
    {
        Value = value;
    }

    public static CartId FromValue(Guid value)
    {
        return new(value);
    }

    public static CartId CreateUnique()
    {
        return new(Guid.NewGuid());
    }
}