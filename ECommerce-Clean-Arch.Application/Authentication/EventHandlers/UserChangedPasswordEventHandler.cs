using ECommerce_Clean_Arch.Application.Persistence;
using ECommerce_Clean_Arch.Application.Persistence.Repositories;
using ECommerce_Clean_Arch.Domain.RefreshTokens.Enums;
using ECommerce_Clean_Arch.Domain.Users.Events;

using MediatR;

namespace ECommerce_Clean_Arch.Application.Authentication.EventHandlers;

public sealed class UserChangedPasswordEventHandler
    : INotificationHandler<UserChangedPassword>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserChangedPasswordEventHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork
    )
    {
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UserChangedPassword notification, CancellationToken cancellationToken)
    {
        await _refreshTokenRepository.RevokeAllByUserIdAsync(
            notification.AggregateId,
            RevokedReason.ChangedPassword,
            cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}