using ECommerce_Clean_Arch.Domain.RefreshTokens;
using ECommerce_Clean_Arch.Domain.RefreshTokens.Enums;

namespace ECommerce_Clean_Arch.Application.Persistence.Repositories;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets token by the token value , ipAddress, and user agent ...
    /// returns null if token not found
    /// </summary>
    /// <param name="opaqueToken"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<RefreshToken?> GetByTokenValueAsync(
        string opaqueToken,
        CancellationToken cancellationToken = default
    );

    Task RevokeAllByUserIdAsync(
        Guid userId,
        RevokedReason revokedReason,
        CancellationToken cancellationToken = default
    );
}