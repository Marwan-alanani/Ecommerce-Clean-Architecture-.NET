using ECommerce_Clean_Arch.Domain.Common.Interfaces;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Domain.Orders.ValueObjects;

public readonly record struct OrderItemId : IStronglyTypedId<OrderItemId, Guid>
{
    public Guid Value { get; }


    private OrderItemId(Guid value)
    {
        Value = value;
    }

    public static OrderItemId CreateUnique() => new(Guid.NewGuid());
    public static OrderItemId FromValue(Guid value) => new(value);

    public static Result<OrderItemId> FromString(string value)
    {
        if (!Guid.TryParse(value, out _))
        {
            return Error.Validation();
        }

        return new OrderItemId(Guid.Parse(value));
    }
}