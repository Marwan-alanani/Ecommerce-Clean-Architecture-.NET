using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Orders;

public sealed record InvalidShippingAddress() : ErrorReason(
    nameof(InvalidShippingAddress),
    "Invalid shipping address"
);