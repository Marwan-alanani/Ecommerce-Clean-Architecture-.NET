using ECommerce_Clean_Arch.Domain.Carts.ValueObjects;

using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Carts;

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