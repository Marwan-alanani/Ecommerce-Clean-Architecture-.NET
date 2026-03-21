using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Carts.Models;

public sealed class Cart
{
    private Dictionary<Guid, CartItem> _items = new();


    public IReadOnlyDictionary<Guid, CartItem> Items
    {
        get => _items;
        set
        {
            _items = value.ToDictionary();
        }
    }

    private Cart()
    {
    }


    public static Cart Create() => new();


    public Result SetCartItem(ProductData product, int quantity)
    {
        var cartItem = _items.GetValueOrDefault(product.Id);
        if (cartItem != null)
        {
            return cartItem.SetQuantity(quantity);
        }

        var itemResult = CartItem.FromProductWithQuantity(product, quantity);
        if (itemResult.IsFailure)
        {
            return itemResult.Error;
        }

        _items.Add(product.Id, itemResult.Value);
        return Result.Success();
    }


    public void RemoveItem(Guid productId)
    {
        _items.Remove(productId);
    }

    public void SetCartItem(Guid productId, CartItem cartItem)
    {
        _items[productId] = cartItem;
    }
}