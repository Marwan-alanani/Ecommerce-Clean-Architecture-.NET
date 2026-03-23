using ECommerce_Clean_Arch.Domain.Orders.ValueObjects;

namespace ECommerce_Clean_Arch.Application.Orders.Queries.GetById;

public sealed record OrderDto(
    Guid Id,
    decimal TotalAmount,
    string Currency,
    List<OrderItemDto> Items,
    string OrderStatus,
    ShippingAddress ShippingAddress,
    DateTime CreatedAt,
    DateTime? ConfirmedAt,
    DateTime? CancelledAt
);

public sealed record OrderItemDto(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    string Currency,
    string PictureUrl,
    int Quantity,
    decimal TotalPrice
);