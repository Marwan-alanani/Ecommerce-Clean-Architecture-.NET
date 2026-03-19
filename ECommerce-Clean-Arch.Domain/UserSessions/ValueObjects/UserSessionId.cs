using ECommerce_Clean_Arch.Domain.Common.Interfaces;

namespace ECommerce_Clean_Arch.Domain.UserSessions.ValueObjects;

public readonly record struct UserSessionId : IAggregateRootId<UserSessionId>
{
    public Guid Value { get; }

    public UserSessionId(Guid value)
    {
        Value = value;
    }

    public static UserSessionId CreateUnique() => new(Guid.NewGuid());
    public static UserSessionId FromValue(Guid value) => new(value);
}