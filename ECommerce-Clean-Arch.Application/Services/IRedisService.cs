using ECommerce_Clean_Arch.Domain.Common.Interfaces;
using ECommerce_Clean_Arch.Domain.Common.Models;

namespace ECommerce_Clean_Arch.Application.Services;

public interface IRedisService
{
    public Task SetAsync<TKey, TValue>(Entity<TKey, TValue> entity, int expirationInMinutes)
        where TValue : IEquatable<TValue>
        where TKey : struct, IStronglyTypedId<TKey, TValue>;

    public Task SetAsync<T>(
        string key,
        T value,
        int expirationInMinutes
    );

    public Task<TEntity?> GetAsync<TEntity, TKey, TValue>(TKey key)
        where TEntity : Entity<TKey, TValue>
        where TKey : struct, IStronglyTypedId<TKey, TValue>
        where TValue : IEquatable<TValue>;

    public Task<T?> GetAsync<T>(string key)
        where T : class;
}