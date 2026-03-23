namespace ECommerce_Clean_Arch.Application.Orders.Commands.Checkout;

public sealed record CheckoutResponse(Guid OrderId, string CheckoutUrl);