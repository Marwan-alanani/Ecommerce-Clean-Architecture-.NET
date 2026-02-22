namespace ECommerce_Clean_Arch.Application.Services;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}