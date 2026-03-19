using ECommerce_Clean_Arch.Domain.Carts.ValueObjects;
using ECommerce_Clean_Arch.Domain.Common.Models;
using ECommerce_Clean_Arch.Domain.Products;
using ECommerce_Clean_Arch.Domain.Products.ValueObjects;

using SharedKernel.Errors;
using SharedKernel.Models;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Domain.Carts.Entities;

public sealed class CartItem : Entity<CartItemId, Guid>
{
    public ProductId ProductId { get; private set; }
    public string ProductName { get; private set; }
    public Money UnitPrice { get; private set; }
    public int Quantity { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public CartItem()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
    }

    private CartItem(
        CartItemId id,
        ProductId productId,
        string productName,
        Money unitPrice,
        int quantity
    ) : base(id)
    {
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public static Result<CartItem> Create(
        ProductId productId,
        string productName,
        Money unitPrice,
        int quantity
    )
    {
        if (quantity <= 0)
        {
            return Error.Validation();
        }

        return new CartItem(
            CartItemId.CreateUnique(),
            productId,
            productName,
            unitPrice,
            quantity
        );
    }

    public static Result<CartItem> FromProductWithQuantity(Product product, int quantity)
    {
        if (quantity <= 0)
        {
            return Error.Validation();
        }

        return new CartItem(
            CartItemId.CreateUnique(),
            product.Id,
            product.Name,
            product.Price,
            quantity
        );
    }

    public void IncreaseQuantity(int quantity)
    {
        Quantity += quantity;
    }

    public Result DecreaseQuantity(int quantity)
    {
        if (Quantity - quantity <= 0)
        {
            return Error.Validation();
        }

        Quantity -= quantity;
        return Result.Success();
    }

    public Result SetQuantity(int quantity)
    {
        if (Quantity <= 0)
        {
            return Error.Validation();
        }

        Quantity = quantity;
        return Result.Success();
    }
}