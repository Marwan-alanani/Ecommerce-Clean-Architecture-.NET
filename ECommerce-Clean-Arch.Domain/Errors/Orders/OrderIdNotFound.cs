using ECommerce_Clean_Arch.Domain.Orders.ValueObjects;

using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Orders;

public sealed record OrderIdNotFound : ErrorReason
{
    public OrderIdNotFound() : base(
        nameof(OrderIdNotFound),
        "No order id in the request metadata",
        nameof(OrderId)
    )
    {
    }
}