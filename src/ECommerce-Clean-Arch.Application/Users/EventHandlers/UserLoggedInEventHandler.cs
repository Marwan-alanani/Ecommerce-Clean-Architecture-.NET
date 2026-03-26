namespace ECommerce_Clean_Arch.Application.Users.EventHandlers;

public sealed class UserLoggedInEventHandler : IDomainEventHandler<UserLoggedInEvent>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartKeyResolver _cartKeyResolver;

    public UserLoggedInEventHandler(
        ICartRepository cartRepository,
        ICartKeyResolver cartKeyResolver
    )
    {
        _cartRepository = cartRepository;
        _cartKeyResolver = cartKeyResolver;
    }

    public async Task Handle(UserLoggedInEvent notification, CancellationToken cancellationToken)
    {
        var userKey = _cartKeyResolver.GetUserKey(notification.AggregateId);
        var guestKey = _cartKeyResolver.GetGuestKey(notification.GuestId);
        await _cartRepository.MergeCartAsync(guestKey, userKey);
    }
}