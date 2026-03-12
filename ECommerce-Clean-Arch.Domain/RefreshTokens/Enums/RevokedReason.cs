namespace ECommerce_Clean_Arch.Domain.RefreshTokens.Enums;

public enum RevokedReason
{
    SessionLimitExceeded,
    TokenRotated,
    UserDeleted,
    SecurityBreach,
    UserLoggedOut,
}

public static class RevokedReasonExtensions
{
    public static string ToStorageString(this RevokedReason reason)
    {
        return reason switch
        {
            RevokedReason.SessionLimitExceeded => "Session limit Exceeded",
            RevokedReason.TokenRotated => "Token rotated",
            RevokedReason.UserDeleted => "User deleted",
            RevokedReason.SecurityBreach => "Security breach",
            RevokedReason.UserLoggedOut => "User logged out",
            _ => throw new ArgumentOutOfRangeException(
                nameof(reason),
                reason,
                null)
        };
    }
}