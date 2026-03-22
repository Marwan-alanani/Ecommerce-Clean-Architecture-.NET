using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Orders;

public record EmptyCart() : ErrorReason(nameof(EmptyCart), "Cannot create order out of empty cart");