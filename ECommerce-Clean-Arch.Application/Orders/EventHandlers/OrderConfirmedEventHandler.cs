namespace ECommerce_Clean_Arch.Application.Orders.EventHandlers;

public sealed class OrderConfirmedEventHandler : IDomainEventHandler<OrderConfirmedEvent>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartKeyResolver _keyResolver;

    public OrderConfirmedEventHandler(ICartRepository cartRepository, ICartKeyResolver keyResolver)
    {
        _cartRepository = cartRepository;
        _keyResolver = keyResolver;
    }

    public async Task Handle(OrderConfirmedEvent notification, CancellationToken cancellationToken)
    {
        await _cartRepository.RemoveCartAsync(_keyResolver.GetUserKey(notification.UserId));
    }
}