using ECommerce_Clean_Arch.Domain.Carts.Entities;
using ECommerce_Clean_Arch.Domain.Carts.ValueObjects;
using ECommerce_Clean_Arch.Domain.Common.Interfaces;
using ECommerce_Clean_Arch.Domain.Common.Models;
using ECommerce_Clean_Arch.Domain.Products;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Domain.Carts;

public sealed class Cart : AggregateRoot<CartId>, IAuditableBase
{
    private readonly List<CartItem> _items = new();

    public Guid UserId { get; private set; }
    public IReadOnlyList<CartItem> Items => _items.ToList();
    public DateTime CreatedAt { get; set; }
    public DateTime LastModifiedAt { get; set; }

    // ReSharper disable once UnusedMember.Local
    public Cart()
    {
    }

    private Cart(CartId id, Guid userId) : base(id)
    {
        UserId = userId;
    }

    public static Cart Create(Guid userId) => new(CartId.CreateUnique(), userId);


    public Result<CartItemId> AddCartItem(Product product, int quantity)
    {
        if (_items.Any(i => i.ProductId == product.Id))
        {
            return Error.Conflict();
        }

        var itemResult = CartItem.FromProductWithQuantity(product, quantity);
        if (itemResult.IsFailure)
        {
            return itemResult.Error;
        }

        _items.Add(itemResult.Value);
        return itemResult.Value.Id;
    }


    public Result ChangeItemQuantity(CartItemId itemId, int quantity)
    {
        var item = _items.FirstOrDefault(i => i.Id == itemId);

        if (item == null)
        {
            return Error.NotFound();
        }

        return item.SetQuantity(quantity);
    }

    public void RemoveItem(CartItemId itemId)
    {
        _items.RemoveAll(i => i.Id == itemId);
    }
}