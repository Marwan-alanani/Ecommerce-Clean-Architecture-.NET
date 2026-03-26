namespace ECommerce_Clean_Arch.Application.Orders.Commands.Checkout.Dtos;

public sealed record CheckoutResult(string SessionId, string SessionUrl);