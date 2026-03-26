namespace ECommerce_Clean_Arch.Application.Orders.Commands.Checkout.Dtos;

public sealed record CheckoutResponse(Guid OrderId, string CheckoutUrl);