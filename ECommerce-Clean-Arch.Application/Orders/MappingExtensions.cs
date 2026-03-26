namespace ECommerce_Clean_Arch.Application.Orders;

public static class MappingExtensions
{
    public static OrderDto ToDto(this Order order)
    {
        return new(
            order.Id.Value,
            order.Total,
            order.Currency,
            order.Items.Select(i => i.ToDto()).ToList(),
            order.Status.ToString(),
            order.ShippingAddress,
            order.CreatedAt,
            order.ConfirmedAt,
            order.CancelledAt
        );
    }

    private static OrderItemDto ToDto(this OrderItem orderItem)
    {
        return new(
            orderItem.ProductId,
            orderItem.ProductName,
            orderItem.UnitPrice.Amount,
            orderItem.UnitPrice.Currency,
            orderItem.PictureUrl,
            orderItem.Quantity,
            orderItem.TotalPrice
        );
    }
}