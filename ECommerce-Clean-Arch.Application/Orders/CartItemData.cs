using SharedKernel.Models;

namespace ECommerce_Clean_Arch.Application.Orders;

public class CartItemData
{
    public MoneyFlat UnitPrice { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string PictureUrl { get; init; } = null!;
    public int Quantity { get; init; }
}