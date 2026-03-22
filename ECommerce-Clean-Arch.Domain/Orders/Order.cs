using ECommerce_Clean_Arch.Domain.Carts;
using ECommerce_Clean_Arch.Domain.Common.Models;
using ECommerce_Clean_Arch.Domain.Errors.Orders;
using ECommerce_Clean_Arch.Domain.Orders.Entities;
using ECommerce_Clean_Arch.Domain.Orders.Enums;
using ECommerce_Clean_Arch.Domain.Orders.Events;
using ECommerce_Clean_Arch.Domain.Orders.ValueObjects;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Domain.Orders;

public class Order : AggregateRoot<OrderId>
{
    private readonly List<OrderItem> _items = new();

    public Guid UserId { get; private set; }
    public ShippingAddress ShippingAddress { get; private set; } = null!;
    public OrderStatus Status { get; private set; }
    public string? SessionId { get; private set; } // to correlate webhook
    public DateTime CreatedAt { get; private set; }
    public DateTime? ConfirmedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }

    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();
    public decimal Total => _items.Sum(i => i.TotalPrice);
    public string Currency => _items.First().UnitPrice.Currency;

    private Order() { } // EF Core

    public static Result<Order> Create(
        Guid userId,
        ShippingAddress shippingAddress,
        Cart cart,
        DateTime utcNow
    )
    {
        if (cart.Items.Count == 0)
            return Error.Validation(new EmptyCart());

        var order = new Order
        {
            Id = OrderId.CreateUnique(),
            UserId = userId,
            ShippingAddress = shippingAddress,
            Status = OrderStatus.Pending,
            CreatedAt = utcNow
        };

        foreach (var (_, cartItem) in cart.Items)
            order._items.Add(OrderItem.FromCartItem(cartItem));
        return order;
    }

    public void AssignStripeSession(string sessionId)
    {
        SessionId = sessionId;
    }

    public Result Confirm(DateTime utcNow)
    {
        if (Status != OrderStatus.Pending)
            return Error.Validation(new OrderNotPending());

        Status = OrderStatus.Confirmed;
        ConfirmedAt = utcNow;

        AddDomainEvent(new OrderConfirmedEvent(UserId));

        return Result.Success();
    }

    public Result Cancel(DateTime utcNow)
    {
        if (Status == OrderStatus.Confirmed)
            return Error.Validation(new OrderAlreadyConfirmed());

        if (Status == OrderStatus.Cancelled)
            return Error.Validation(new OrderAlreadyCancelled());

        Status = OrderStatus.Cancelled;
        CancelledAt = utcNow;


        return Result.Success();
    }
}