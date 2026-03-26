

namespace ECommerce_Clean_Arch.Application.Abstractions.Persistence.Repositories;

public interface ISessionRepository
{
    Task AddAsync(
        UserSession userSession,
        string tokenValue,
        CancellationToken cancellationToken = default
    );

    Task RevokeAllByUserIdAsync(
        Guid userId,
        RevokedReason revokedReason,
        CancellationToken cancellationToken = default
    );

    Task<SessionData?> GetSessionDataByRefreshTokenAsync(
        string opaqueToken,
        CancellationToken cancellationToken = default
    );

    Task RefreshTokenAsync(
        string oldToken,
        string newToken,
        Guid userId
    );

    Task RevokeByValueAsync(
        string tokenValue,
        RevokedReason revokedReason,
        CancellationToken cancellationToken = default
    );
}