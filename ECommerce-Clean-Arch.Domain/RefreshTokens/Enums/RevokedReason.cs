namespace ECommerce_Clean_Arch.Domain.RefreshTokens.Enums;

public enum RevokedReason
{
    SessionLimitExceeded,
    TokenRotated,
    UserDeleted,
    SecurityBreach,
    LoggedOut,
    LoggedOutAll,
    ChangedPassword,
}