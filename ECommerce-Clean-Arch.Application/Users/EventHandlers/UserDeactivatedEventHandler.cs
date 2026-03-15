using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Persistence;
using ECommerce_Clean_Arch.Application.Persistence.Repositories;
using ECommerce_Clean_Arch.Domain.RefreshTokens.Enums;
using ECommerce_Clean_Arch.Domain.Users.Events;

namespace ECommerce_Clean_Arch.Application.Users.EventHandlers;

public class UserDeactivatedEventHandler : IDomainEventHandler<UserDeactivatedEvent>
{
    private readonly IRefreshTokenRepository _tokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserDeactivatedEventHandler(IRefreshTokenRepository tokenRepository, IUnitOfWork unitOfWork)
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