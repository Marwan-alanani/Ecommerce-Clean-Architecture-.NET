using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Orders;

public sealed record OrderAlreadyConfirmed() : ErrorReason(
    nameof(OrderAlreadyConfirmed),
    "Order is already confirmed");