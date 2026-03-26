using ECommerce_Clean_Arch.Application.Abstractions.Services;

namespace ECommerce_Clean_Arch.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}