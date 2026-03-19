using ECommerce_Clean_Arch.Domain.Common.Models;
using ECommerce_Clean_Arch.Domain.UserSessions.Enums;
using ECommerce_Clean_Arch.Domain.UserSessions.ValueObjects;

namespace ECommerce_Clean_Arch.Domain.UserSessions;

public sealed class UserSession : AggregateRoot<UserSessionId>
{
    public Guid UserId { get; private set; }
    public string UserAgent { get; private set; }
    public string? IpAddress { get; private set; }
    public DateTime IssuedAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public RevokedReason? RevokedReason { get; private set; }

    private UserSession(
        UserSessionId id,
        Guid userId,
        string userAgent,
        string? ipAddress,
        DateTime issuedAt,
        DateTime? revokedAt,
        RevokedReason? revokedReason
    ) : base(id)
    {
        UserId = userId;
        UserAgent = userAgent;
        IpAddress = ipAddress;
        IssuedAt = issuedAt;
        RevokedAt = revokedAt;
        RevokedReason = revokedReason;
    }

    public static UserSession Create(
        Guid userId,
        string userAgent,
        string? ipAddress,
        DateTime issuedAt
    )
    {
        return new(
            UserSessionId.CreateUnique(),
            userId,
            userAgent,
            ipAddress,
            issuedAt,
            null,
            null
        );
    }

    public void Revoke(RevokedReason revokedReason, DateTime revokedAt)
    {
        RevokedAt = revokedAt;
        RevokedReason = revokedReason;
        Deactivate();
    }
}