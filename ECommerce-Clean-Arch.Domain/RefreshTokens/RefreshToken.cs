using ECommerce_Clean_Arch.Domain.Common.Models;
using ECommerce_Clean_Arch.Domain.RefreshTokens.Enums;

namespace ECommerce_Clean_Arch.Domain.RefreshTokens;

public sealed class RefreshToken : AggregateRoot<Guid>
{
#pragma warning disable CS8618
    private RefreshToken(
    )
    {
    }
#pragma warning restore CS8618
    public const string CookieName = "refreshToken";


    private RefreshToken(
        Guid id,
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
    public string? RevokedReason { get; private set; }

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
            Guid.NewGuid(),
            userId,
            tokenHash,
            userAgent,
            ipAddress,
            createdAt
        );
    }


    public void Revoke(RevokedReason reason, DateTime utcNow)
    {
        RevokedReason = reason.ToStorageString();
        RevokedAt = utcNow;
    }

    public bool IsExpired(DateTime utcNow) => ExpiresOnUtc < utcNow;
    public void SetExpiresOnUtc(DateTime expiryDateTime) => ExpiresOnUtc = expiryDateTime;
}