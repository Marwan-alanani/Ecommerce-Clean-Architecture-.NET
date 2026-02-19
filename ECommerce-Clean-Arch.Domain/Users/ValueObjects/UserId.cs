namespace ECommerce_Clean_Arch.Domain.Users.ValueObjects;

public record struct UserId(
    Guid Value
)
{
    public static UserId CreateUnique()
    {
        return new UserId(Guid.NewGuid());
    }

    public static UserId Create(string value)
    {
        return new UserId(Guid.Parse(value));
    }
}