namespace ECommerce_Clean_Arch.Domain.UserSessions.Enums;

public enum RevokedReason
{
    SessionLimitExceeded,
    SecurityBreach,
    LoggedOut,
    LoggedOutAll,
    ChangedPassword,
    UserDeactivated
}