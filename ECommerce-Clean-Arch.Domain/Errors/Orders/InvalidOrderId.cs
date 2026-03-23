using ECommerce_Clean_Arch.Domain.Orders.ValueObjects;

using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Orders;

public sealed record InvalidOrderId : ErrorReason
{
    public InvalidOrderId(
        string value
    ) : base(
        nameof(InvalidOrderId),
        $"{value} is not a valid order id.",
        nameof(OrderId))
    {
    }
}