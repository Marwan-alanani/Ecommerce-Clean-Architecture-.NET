namespace ECommerce_Clean_Arch.Domain.RefreshTokens.Enums;

public enum RevokedReason
{
    SessionLimitExceeded,
    TokenRotated,
    UserDeactivated,
    SecurityBreach,
    LoggedOut,
    LoggedOutAll,
    ChangedPassword,
}