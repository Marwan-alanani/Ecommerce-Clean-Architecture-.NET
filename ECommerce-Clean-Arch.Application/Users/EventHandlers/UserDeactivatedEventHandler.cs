using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence.Repositories;
using ECommerce_Clean_Arch.Domain.RefreshTokens.Enums;
using ECommerce_Clean_Arch.Domain.Users.Events;

namespace ECommerce_Clean_Arch.Application.Users.EventHandlers;

public class UserDeactivatedEventHandler : IDomainEventHandler<UserDeactivatedEvent>
{
    private readonly IRefreshTokenRepository _tokenRepository;
    private readonly IApplicationDbContext _unitOfWork;

    public UserDeactivatedEventHandler(
        IRefreshTokenRepository tokenRepository,
        IApplicationDbContext unitOfWork
    )
    {
        _tokenRepository = tokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UserDeactivatedEvent notification, CancellationToken cancellationToken)
    {
        await _tokenRepository.RevokeAllByUserIdAsync(
            notification.AggregateId,
            RevokedReason.UserDeactivated,
            cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}