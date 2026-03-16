using ECommerce_Clean_Arch.Application.Abstractions.Persistence.Repositories;
using ECommerce_Clean_Arch.Application.Authentication.Services;
using ECommerce_Clean_Arch.Application.Services;
using ECommerce_Clean_Arch.Domain.RefreshTokens;
using ECommerce_Clean_Arch.Domain.RefreshTokens.Enums;
using ECommerce_Clean_Arch.Infrastructure.Authentication;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ECommerce_Clean_Arch.Infrastructure.Persistence.Repositories;

public sealed class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IDateTimeProvider _dateTime;
    private readonly ITokenProvider _tokenProvider;
    private readonly JwtConfig _jwtConfig;
    private const int MaxTokensPerUser = 5;

    public RefreshTokenRepository(
        ApplicationDbContext context,
        IDateTimeProvider dateTime,
        ITokenProvider tokenProvider,
        IOptions<JwtConfig> jwtConfig
    )
    {
        _context = context;
        _dateTime = dateTime;
        _tokenProvider = tokenProvider;
        _jwtConfig = jwtConfig.Value;
    }

    public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        // 1. Get all valid tokens for this user
        var validTokens = await _context.Set<RefreshToken>()
            .Where(rt => rt.UserId == refreshToken.UserId)
            .Where(rt => rt.RevokedAt == null && rt.ExpiresOnUtc > _dateTime.UtcNow)
            .OrderBy(rt => rt.ExpiresOnUtc) // Oldest expiry first
            .ToListAsync(cancellationToken);

        // 2. Check if at limit
        if (validTokens.Count >= MaxTokensPerUser)
        {
            var tokensToRemove = validTokens
                .Take(validTokens.Count - MaxTokensPerUser + 1) // Remove enough to make room
                .ToList();

            foreach (var token in tokensToRemove)
            {
                token.Revoke(RevokedReason.SessionLimitExceeded, _dateTime.UtcNow);
            }
        }

        refreshToken.SetExpiresOnUtc(_dateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenExpiryInDays));
        // 4. Add the new token
        await _context.Set<RefreshToken>().AddAsync(refreshToken, cancellationToken);
    }

    public async Task<RefreshToken?> GetByTokenValueAsync(
        string opaqueToken,
        CancellationToken cancellationToken = default
    )
    {
        var tokenHash = _tokenProvider.HashOpaqueToken(opaqueToken);
        return await _context.Set<RefreshToken>()
            .FirstOrDefaultAsync(
                r => r.TokenHash == tokenHash,
                cancellationToken: cancellationToken
            );
    }

    public async Task RevokeAllByUserIdAsync(
        Guid userId,
        RevokedReason revokedReason,
        CancellationToken cancellationToken = default
    )
    {
        var activeRefreshTokens = await _context.Set<RefreshToken>()
            .Where(r => r.UserId == userId)
            .Where(r => r.RevokedReason == null)
            .ToListAsync(cancellationToken);
        foreach (var refreshToken in activeRefreshTokens)
        {
            refreshToken.Revoke(revokedReason, _dateTime.UtcNow);
        }
    }
}