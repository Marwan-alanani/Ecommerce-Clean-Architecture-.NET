using ECommerce_Clean_Arch.Domain.Errors.Carts;

using SharedKernel.Errors;
using SharedKernel.Models;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Domain.Carts.ValueObjects;

public sealed class CartItem
{
    private int _quantity;
    public string Name { get; init; }
    public Guid ProductId { get; init; }
    public MoneyFlat UnitPrice { get; init; }

    public int Quantity
    {
        get => _quantity;
        init
        {
            _quantity = value;
        }
    }

    public string PictureUrl { get; init; }

    private CartItem()
    {
    }

    private CartItem(
        string name,
        MoneyFlat unitPrice,
        int quantity,
        string pictureUrl,
        Guid productId
    )
    {
        Name = name;
        UnitPrice = unitPrice;
        _quantity = quantity;
        PictureUrl = pictureUrl;
        ProductId = productId;
    }

    public static Result<CartItem> FromProductWithQuantity(ProductData product, int quantity)
    {
        if (quantity <= 0)
        {
            return Error.Validation(new InvalidItemQuantity(quantity));
        }

        return new CartItem(
            product.Name,
            product.Price,
            quantity,
            product.PictureUrl,
            product.Id
        );
    }


    public Result SetQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            return Error.Validation(new InvalidItemQuantity(quantity));
        }

        _quantity = quantity;
        return Result.Success();
    }
}