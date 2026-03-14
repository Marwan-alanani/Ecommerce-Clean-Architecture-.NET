using ECommerce_Clean_Arch.Domain.Common.Interfaces;

namespace ECommerce_Clean_Arch.Domain.RefreshTokens.ValueObjects;

public readonly record struct RefreshTokenId : IAggregateRootId<RefreshTokenId>
{
    public Guid Value { get; }

    private RefreshTokenId(Guid value)
    {
        Value = value;
    }

    public static RefreshTokenId CreateUnique()
    {
        return new RefreshTokenId(Guid.NewGuid());
    }

    public static RefreshTokenId FromValue(Guid value)
    {
        return new RefreshTokenId(value);
    }
}