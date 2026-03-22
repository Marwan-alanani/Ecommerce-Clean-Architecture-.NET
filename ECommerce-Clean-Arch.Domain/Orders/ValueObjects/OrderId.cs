using ECommerce_Clean_Arch.Domain.Common.Interfaces;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Domain.Orders.ValueObjects;

public readonly record struct OrderId : IAggregateRootId<OrderId>
{
    public Guid Value { get; }

    private OrderId(Guid value)
    {
        Value = value;
    }

    public static OrderId CreateUnique() => new(Guid.NewGuid());
    public static OrderId FromValue(Guid value) => new(value);

    public static Result<OrderId> FromString(string value)
    {
        if (!Guid.TryParse(value, out _))
        {
            return Error.Validation();
        }

        return new OrderId(Guid.Parse(value));
    }
}