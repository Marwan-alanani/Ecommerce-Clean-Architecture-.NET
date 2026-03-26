using ECommerce_Clean_Arch.Domain.Orders;
using ECommerce_Clean_Arch.Domain.Orders.ValueObjects;

using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Orders;

public sealed record OrderNotFound : ErrorReason
{
    public OrderNotFound(OrderId id) : base(
        nameof(OrderNotFound),
        $"No order with the given Id:{id.Value} was found.",
        nameof(Order.Id))
    {
    }
}