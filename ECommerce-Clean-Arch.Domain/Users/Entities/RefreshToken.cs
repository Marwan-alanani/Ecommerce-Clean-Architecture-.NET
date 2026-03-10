using ECommerce_Clean_Arch.Domain.Common.Models;

namespace ECommerce_Clean_Arch.Domain.Users.Entities;

public sealed class RefreshToken : Entity<Guid>
{
#pragma warning disable CS8618
    private RefreshToken() { }
#pragma warning restore CS8618


    private RefreshToken(
        Guid id,
        string hashedValue,
        DateTime expiresOnUtc
    ) : base(id)
    {
        HashedValue = hashedValue;
        ExpiresOnUtc = expiresOnUtc;
    }

    public string HashedValue { get; private set; }
    public DateTime ExpiresOnUtc { get; private set; }

    public static RefreshToken Create(
        string hashedValue,
        DateTime expiresOnUtc
    )
    {
        return new(
            Guid.NewGuid(),
            hashedValue,
            expiresOnUtc
        );
    }
}