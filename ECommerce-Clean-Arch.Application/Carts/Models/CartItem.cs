using ECommerce_Clean_Arch.Application.Carts.Errors;
using ECommerce_Clean_Arch.Application.Common.Models;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Carts.Models;

public sealed class CartItem
{
    public string Name { get; init; }
    public Guid ProductId { get; init; }
    public MoneyDto UnitPrice { get; init; }
    public int Quantity { get; private set; }
    public string PictureUrl { get; init; }

    private CartItem()
    {
    }

    private CartItem(
        string name,
        MoneyDto unitPrice,
        int quantity,
        string pictureUrl,
        Guid productId
    )
    {
        Name = name;
        UnitPrice = unitPrice;
        Quantity = quantity;
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

        Quantity = quantity;
        return Result.Success();
    }
}