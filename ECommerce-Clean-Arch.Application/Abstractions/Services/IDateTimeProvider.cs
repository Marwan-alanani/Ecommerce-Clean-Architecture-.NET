namespace ECommerce_Clean_Arch.Application.Abstractions.Services;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}