using ECommerce_Clean_Arch.Domain.Common.Interfaces;

namespace ECommerce_Clean_Arch.Domain.Carts.ValueObjects;

public readonly record struct CartItemId : IStronglyTypedId<CartItemId, Guid>
{
    public Guid Value { get; }

    private CartItemId(Guid value)
    {
        Value = value;
    }

    public static CartItemId FromValue(Guid value) => new(value);
    public static CartItemId CreateUnique() => new(Guid.NewGuid());
}