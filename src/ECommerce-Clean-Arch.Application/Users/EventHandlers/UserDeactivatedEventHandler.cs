namespace ECommerce_Clean_Arch.Application.Users.EventHandlers;

public class UserDeactivatedEventHandler : IDomainEventHandler<UserDeactivatedEvent>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IApplicationDbContext _context;

    public UserDeactivatedEventHandler(
        ISessionRepository sessionRepository,
        IApplicationDbContext context
    )
    {
        _sessionRepository = sessionRepository;
        _context = context;
    }

    public async Task Handle(UserDeactivatedEvent notification, CancellationToken cancellationToken)
    {
        await _sessionRepository.RevokeAllByUserIdAsync(
            notification.AggregateId,
            RevokedReason.UserDeactivated,
            cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}