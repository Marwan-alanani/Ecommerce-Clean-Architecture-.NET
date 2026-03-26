using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Orders;

public sealed record OrderNotPending() : ErrorReason(
    nameof(OrderNotPending),
    "Order is not " +
    "in pending state");