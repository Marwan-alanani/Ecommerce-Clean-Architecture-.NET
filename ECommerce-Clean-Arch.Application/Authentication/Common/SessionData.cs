namespace ECommerce_Clean_Arch.Application.Authentication.Common;

public sealed class SessionData
{
    public SessionData()
    {
    }

    public SessionData(
        Guid sessionId,
        Guid userId
    )
    {
        SessionId = sessionId;
        UserId = userId;
    }

    public Guid SessionId { get; init; }
    public Guid UserId { get; init; }
}