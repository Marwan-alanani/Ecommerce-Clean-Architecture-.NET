using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Orders;

public sealed record OrderAlreadyCancelled() : ErrorReason(
    nameof(OrderAlreadyCancelled),
    "Order is already cancelled"
);