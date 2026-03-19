using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence.Repositories;
using ECommerce_Clean_Arch.Domain.Users.Events;
using ECommerce_Clean_Arch.Domain.UserSessions.Enums;


namespace ECommerce_Clean_Arch.Application.Authentication.EventHandlers;

public sealed class UserChangedPasswordEventHandler
    : IDomainEventHandler<UserChangedPasswordEvent>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IApplicationDbContext _unitOfWork;

    public UserChangedPasswordEventHandler(
        ISessionRepository sessionRepository,
        IApplicationDbContext unitOfWork
    )
    {
        _sessionRepository = sessionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UserChangedPasswordEvent notification, CancellationToken cancellationToken)
    {
        await _sessionRepository.RevokeAllByUserIdAsync(
            notification.AggregateId,
            RevokedReason.ChangedPassword,
            cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}