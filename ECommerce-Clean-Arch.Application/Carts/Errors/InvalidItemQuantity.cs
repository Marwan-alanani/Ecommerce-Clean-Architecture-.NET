using ECommerce_Clean_Arch.Application.Carts.Models;

using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Application.Carts.Errors;

public sealed record InvalidItemQuantity : ErrorReason
{
    public InvalidItemQuantity(int quantity) : base(
        nameof(InvalidItemQuantity),
        $"Quantity sent : {quantity} , quantity must be greater than zero",
        nameof(CartItem.Quantity)
    )
    {
    }
}