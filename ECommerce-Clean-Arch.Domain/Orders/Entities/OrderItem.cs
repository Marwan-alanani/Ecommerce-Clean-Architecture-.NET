using ECommerce_Clean_Arch.Domain.Carts.ValueObjects;
using SharedKernel.Models;

namespace ECommerce_Clean_Arch.Domain.Orders.Entities;

public sealed class OrderItem
{
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; } = null!; // snapshot
    public MoneyFlat UnitPrice { get; private set; } = null!; // snapshot
    public string PictureUrl { get; private set; } = null!; // snapshot
    public int Quantity { get; private set; }

    public decimal TotalPrice => UnitPrice.Amount * Quantity;

    private OrderItem(
        Guid productId,
        string productName,
        MoneyFlat unitPrice,
        string pictureUrl,
        int quantity
    )
    {
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        PictureUrl = pictureUrl;
        Quantity = quantity;
    }

    // ReSharper disable once UnusedMember.Local
    private OrderItem() // Ef core
    {
    }

    internal static OrderItem FromCartItem(CartItem item)
    {
        return new(
            item.ProductId,
            item.Name,
            item.UnitPrice,
            item.PictureUrl,
            item.Quantity
        );
    }
}