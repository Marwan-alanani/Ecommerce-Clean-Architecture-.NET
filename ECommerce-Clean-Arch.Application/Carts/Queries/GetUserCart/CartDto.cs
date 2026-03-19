using ECommerce_Clean_Arch.Application.Common.Models;
using ECommerce_Clean_Arch.Domain.Carts;
using ECommerce_Clean_Arch.Domain.Carts.Entities;

namespace ECommerce_Clean_Arch.Application.Carts.Queries.GetUserCart;

public sealed record CartDto(
    Guid Id,
    List<CartItemDto> Items,
    DateTime CreatedAt,
    DateTime LastModifiedAt
);

public sealed record CartItemDto(
    Guid Id,
    string Name,
    MoneyDto UnitPrice,
    int Quantity
);

public static class MappingExtensions
{
    private static CartItemDto ToDto(this CartItem cartItem)
    {
        return new(
            cartItem.Id.Value,
            cartItem.ProductName,
            new MoneyDto(cartItem.UnitPrice.Currency.ToString(), cartItem.UnitPrice.Amount),
            cartItem.Quantity
        );
    }

    public static CartDto ToDto(this Cart cart)
    {
        return new(
            cart.Id.Value,
            cart.Items.Select(i => i.ToDto()).ToList(),
            cart.CreatedAt,
            cart.LastModifiedAt
        );
    }
}