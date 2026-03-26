namespace ECommerce_Clean_Arch.Application.Abstractions.Services;

public interface ICacheService
{
    public Task SetAsync<T>(
        string key,
        T value,
        TimeSpan expiration
    );

    public Task<T?> GetAsync<T>(string key);
}