using ECommerce_Clean_Arch.Application.Abstractions.Services;
using ECommerce_Clean_Arch.Domain.Errors.Common.Exceptions;
using ECommerce_Clean_Arch.Infrastructure.Extensions;

using Newtonsoft.Json;

using StackExchange.Redis;


namespace ECommerce_Clean_Arch.Infrastructure.Services;

public sealed class CacheService : ICacheService
{
    private readonly IDatabase _database;

    public CacheService(IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase();
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        TimeSpan expiration
    )
    {
        var json = JsonConvert.SerializeObject(value);
        await _database.StringSetAsync(
            key,
            json,
            expiration
        );
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var json = await _database.StringGetAsync(key);
        if (json.IsNullOrEmpty)
        {
            return default;
        }

        var obj = JsonConvertExtensions.Deserialize<T>(json.ToString());
        if (obj is null) { throw new RedisDeserializationException(nameof(T)); }

        return obj;
    }
}