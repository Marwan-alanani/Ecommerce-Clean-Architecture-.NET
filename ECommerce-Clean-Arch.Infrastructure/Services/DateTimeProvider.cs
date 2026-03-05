using ECommerce_Clean_Arch.Application.Services;

namespace ECommerce_Clean_Arch.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}