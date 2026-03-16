using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence.Repositories;
using ECommerce_Clean_Arch.Domain.RefreshTokens.Enums;
using ECommerce_Clean_Arch.Domain.Users.Events;


namespace ECommerce_Clean_Arch.Application.Authentication.EventHandlers;

public sealed class UserChangedPasswordEventHandler
    : IDomainEventHandler<UserChangedPasswordEvent>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IApplicationDbContext _unitOfWork;

    public UserChangedPasswordEventHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IApplicationDbContext unitOfWork
    )
    {
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UserChangedPasswordEvent notification, CancellationToken cancellationToken)
    {
        await _refreshTokenRepository.RevokeAllByUserIdAsync(
            notification.AggregateId,
            RevokedReason.ChangedPassword,
            cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}