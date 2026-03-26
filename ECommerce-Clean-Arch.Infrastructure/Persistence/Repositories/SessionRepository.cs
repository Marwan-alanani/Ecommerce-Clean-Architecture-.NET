using ECommerce_Clean_Arch.Application.Abstractions.Persistence.Repositories;
using ECommerce_Clean_Arch.Application.Abstractions.Services;
using ECommerce_Clean_Arch.Application.Authentication.Common;
using ECommerce_Clean_Arch.Application.Authentication.Services;
using ECommerce_Clean_Arch.Domain.Errors.Common.Exceptions;
using ECommerce_Clean_Arch.Domain.UserSessions;
using ECommerce_Clean_Arch.Domain.UserSessions.Enums;
using ECommerce_Clean_Arch.Domain.UserSessions.ValueObjects;
using ECommerce_Clean_Arch.Infrastructure.Configurations;
using ECommerce_Clean_Arch.Infrastructure.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using StackExchange.Redis;


namespace ECommerce_Clean_Arch.Infrastructure.Persistence.Repositories;

public sealed class SessionRepository : ISessionRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IDatabase _database;
    private readonly DateTime _utcNow;
    private readonly ITokenProvider _tokenProvider;
    private const int MaxSessionsPerUser = 5;
    private readonly TimeSpan _ttl;

    public SessionRepository(
        ApplicationDbContext context,
        IDateTimeProvider dateTime,
        ITokenProvider tokenProvider,
        IOptions<JwtConfig> jwtConfig,
        IConnectionMultiplexer connectionMultiplexer
    )
    {
        _context = context;
        _utcNow = dateTime.UtcNow;
        _tokenProvider = tokenProvider;
        _database = connectionMultiplexer.GetDatabase();
        _ttl = TimeSpan.FromDays(jwtConfig.Value.RefreshTokenExpiryInDays);
    }

    public async Task AddAsync(
        UserSession userSession,
        string tokenValue,
        CancellationToken cancellationToken = default
    )
    {
        //1. Hash token
        var tokenHash = _tokenProvider.HashOpaqueToken(tokenValue);
        //2. Get valid user tokens sorted by ttl
        var activeTokenHashes = await _database
            .SortedSetRangeByRankAsync(GetUserSessionKey(userSession.UserId));

        // 3. begin redis transaction
        var redisTransaction = _database.CreateTransaction();

        // 4. Check if session limit exceeded
        if (activeTokenHashes.Length >= MaxSessionsPerUser)
        {
            var oldestHashes = activeTokenHashes
                .Take(activeTokenHashes.Length - MaxSessionsPerUser + 1)
                .ToList();

            // revoke from sql
            foreach (var hash in oldestHashes)
            {
                var jsonSessionData =
                    await _database.StringGetAsync(GetRefreshTokenKey(hash.ToString()));

                var sessionDataToRemove = JsonConvertExtensions
                    .Deserialize<SessionData>(jsonSessionData.ToString());

                if (sessionDataToRemove == null)
                    throw new RedisDeserializationException(nameof(SessionData));
                var sessionToRevoke = await _context.UserSessions
                    .Where(s => s.Id == UserSessionId.FromValue(sessionDataToRemove.SessionId))
                    .FirstOrDefaultAsync(cancellationToken);
                sessionToRevoke?.Revoke(RevokedReason.SessionLimitExceeded, _utcNow);
            }

            // remove from redis
            foreach (var hash in oldestHashes)
            {
                _ = redisTransaction.KeyDeleteAsync(GetRefreshTokenKey(hash.ToString()));
                _ = redisTransaction.SortedSetRemoveAsync(
                    GetUserSessionKey(userSession.UserId),
                    hash
                );
            }
        }

        // add new session to redis
        var expiryScore = DateTimeOffset.UtcNow.Add(_ttl).ToUnixTimeSeconds();
        _ = redisTransaction.SortedSetAddAsync(
            GetUserSessionKey(userSession.UserId),
            tokenHash,
            expiryScore
        );
        var sessionData = new SessionData(userSession.Id.Value, userSession.UserId);
        _ = redisTransaction.StringSetAsync(
            GetRefreshTokenKey(tokenHash),
            JsonConvert.SerializeObject(sessionData),
            _ttl);
        // add new session to database
        await _context.UserSessions.AddAsync(userSession, cancellationToken);
        // execute redis transaction
        await redisTransaction.ExecuteAsync();
    }

    private string GetUserSessionKey(Guid userId)
    {
        return $"user-sessions:{userId}";
    }

    private string GetRefreshTokenKey(string tokenHash)
    {
        return $"refresh:{tokenHash}";
    }


    public async Task RevokeAllByUserIdAsync(
        Guid userId,
        RevokedReason revokedReason,
        CancellationToken cancellationToken = default
    )
    {
        var activeRefreshTokenHashes = await _database
            .SortedSetRangeByRankAsync(GetUserSessionKey(userId));
        foreach (var hash in activeRefreshTokenHashes)
        {
            var jsonData = await _database.StringGetDeleteAsync(GetRefreshTokenKey(hash.ToString()));
            if (jsonData.IsNullOrEmpty) continue;
            var sessionData = JsonConvertExtensions.Deserialize<SessionData>(jsonData.ToString());
            if (sessionData == null)
                throw new RedisDeserializationException(nameof(SessionData));
            var userSession = await _context.UserSessions
                .Where(s => s.Id == UserSessionId.FromValue(sessionData.SessionId))
                .FirstOrDefaultAsync(cancellationToken);
            userSession?.Revoke(revokedReason, _utcNow);
        }

        await _database.KeyDeleteAsync(GetUserSessionKey(userId)); // remove sorted set
    }

    public async Task<SessionData?> GetSessionDataByRefreshTokenAsync(
        string opaqueToken,
        CancellationToken cancellationToken = default
    )
    {
        var hash = _tokenProvider.HashOpaqueToken(opaqueToken);
        var jsonData = await _database.StringGetAsync(GetRefreshTokenKey(hash));
        if (jsonData.IsNullOrEmpty)
        {
            return null;
        }

        var sessionData = JsonConvertExtensions.Deserialize<SessionData>(jsonData.ToString());
        if (sessionData == null)
        {
            throw new RedisDeserializationException(nameof(SessionData));
        }

        return sessionData;
    }

    public async Task RefreshTokenAsync(
        string oldToken,
        string newToken,
        Guid userId
    )
    {
        var oldHash = _tokenProvider.HashOpaqueToken(oldToken);
        var newHash = _tokenProvider.HashOpaqueToken(newToken);
        var transaction = _database.CreateTransaction();
        var expiryScore = DateTimeOffset.UtcNow.Add(_ttl).ToUnixTimeSeconds();

        var jsonData = await _database.StringGetDeleteAsync(GetRefreshTokenKey(oldHash));
        _ = transaction.SortedSetRemoveAsync(GetUserSessionKey(userId), oldHash);

        _ = transaction.StringSetAsync(
            GetRefreshTokenKey(newHash),
            jsonData,
            _ttl);
        _ = transaction.SortedSetAddAsync(
            GetUserSessionKey(userId),
            newHash,
            expiryScore
        );
        await transaction.ExecuteAsync();
    }

    public async Task RevokeByValueAsync(
        string tokenValue,
        RevokedReason revokedReason,
        CancellationToken cancellationToken = default
    )
    {
        var hash = _tokenProvider.HashOpaqueToken(tokenValue);
        var jsonData = await _database.StringGetDeleteAsync(GetRefreshTokenKey(hash));
        if (jsonData.IsNullOrEmpty)
        {
            // session already revoked
            return;
        }

        var sessionData = JsonConvertExtensions.Deserialize<SessionData>(jsonData.ToString());
        if (sessionData == null) throw new RedisDeserializationException(nameof(SessionData));

        var userSession = await _context.UserSessions
            .Where(s => s.Id == UserSessionId.FromValue(sessionData.SessionId))
            .FirstOrDefaultAsync(cancellationToken);
        if (userSession == null)
        {
            // doesn't exist in db ... ?
            return;
        }

        userSession.Revoke(revokedReason, _utcNow);
        await _database.SortedSetRemoveAsync(GetUserSessionKey(sessionData.UserId), hash);
    }
}