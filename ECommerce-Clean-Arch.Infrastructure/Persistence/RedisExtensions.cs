using ECommerce_Clean_Arch.Domain.Common.Interfaces;
using ECommerce_Clean_Arch.Domain.Common.Models;

using Newtonsoft.Json;

using StackExchange.Redis;

namespace ECommerce_Clean_Arch.Infrastructure.Persistence;

public static class RedisExtensions
{
    public static async Task SetAsync<TKey, TValue>(
        this IDatabase database,
        Entity<TKey, TValue> entity,
        DateTime expiration
    )
        where TKey : struct, IStronglyTypedId<TKey, TValue>
        where TValue : IEquatable<TValue>
    {
        var key = ResolveKey(entity.Id);
        var jsonData = JsonConvert.SerializeObject(entity);
        await database.StringSetAsync(
            key,
            jsonData,
            expiration
        );
    }


    public static async Task SetAsync<T>(
        this IDatabase database,
        string key,
        T value,
        DateTime expiration
    )
    {
        var jsonData = JsonConvert.SerializeObject(value);
        await database.StringSetAsync(
            key,
            jsonData,
            expiration
        );
    }

    public static async Task<TEntity?> GetAsync<TEntity, TKey, TValue>(
        this IDatabase database,
        TKey key
    )
        where TEntity : Entity<TKey, TValue>
        where TKey : struct, IStronglyTypedId<TKey, TValue>
        where TValue : IEquatable<TValue>
    {
        var jsonData = await database.StringGetAsync(ResolveKey(key));
        if (jsonData.IsNullOrEmpty)
        {
            return null;
        }

        return JsonConvert.DeserializeObject<TEntity>(jsonData.ToString());
    }

    public static async Task<T?> GetAsync<T>(this IDatabase database, string key)
        where T : class
    {
        var jsonData = await database.StringGetAsync(key);
        if (jsonData.IsNullOrEmpty)
        {
            return null;
        }

        return JsonConvert.DeserializeObject<T>(jsonData.ToString());
    }

    private static string ResolveKey<TKey, TValue>(IStronglyTypedId<TKey, TValue> key)
        where TKey : struct, IEquatable<TKey>
        where TValue : IEquatable<TValue>
    {
        var typeName = typeof(TKey).Name;
        return $"{typeName}_{key.Value}";
    }
}