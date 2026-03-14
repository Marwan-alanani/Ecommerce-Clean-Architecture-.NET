using ECommerce_Clean_Arch.Domain.Common.Models;
using ECommerce_Clean_Arch.Domain.RefreshTokens.Enums;
using ECommerce_Clean_Arch.Domain.RefreshTokens.ValueObjects;

namespace ECommerce_Clean_Arch.Domain.RefreshTokens;

public sealed class RefreshToken : AggregateRoot<RefreshTokenId>
{
#pragma warning disable CS8618
    private RefreshToken(
    )
    {
    }
#pragma warning restore CS8618
    public const string CookieName = "refreshToken";


    private RefreshToken(
        RefreshTokenId id,
        Guid userId,
        string tokenHash,
        string userAgent,
        string? ipAddress,
        DateTime createdAt
    ) : base(id)
    {
        UserId = userId;
        TokenHash = tokenHash;
        UserAgent = userAgent;
        IpAddress = ipAddress;
        CreatedAt = createdAt;
    }

    public string TokenHash { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresOnUtc { get; private set; }

    public DateTime? RevokedAt { get; private set; }
    public RevokedReason? RevokedReason { get; private set; }

    public string UserAgent { get; set; }
    public string? IpAddress { get; set; }
    public Guid UserId { get; private set; }

    public static RefreshToken Create(
        Guid userId,
        string tokenHash,
        DateTime createdAt,
        string userAgent,
        string? ipAddress
    )
    {
        return new(
            RefreshTokenId.CreateUnique(),
            userId,
            tokenHash,
            userAgent,
            ipAddress,
            createdAt
        );
    }


    public void Revoke(RevokedReason reason, DateTime utcNow)
    {
        RevokedReason = reason;
        RevokedAt = utcNow;
    }

    public bool IsExpired(DateTime utcNow) => ExpiresOnUtc < utcNow;
    public void SetExpiresOnUtc(DateTime expiryDateTime) => ExpiresOnUtc = expiryDateTime;
}