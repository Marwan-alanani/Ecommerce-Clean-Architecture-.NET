using ECommerce_Clean_Arch.Application.Abstractions.Persistence.Repositories;
using ECommerce_Clean_Arch.Application.Carts.Models;
using ECommerce_Clean_Arch.Application.Common.Interfaces;
using ECommerce_Clean_Arch.Domain.Errors.Common.Exceptions;
using ECommerce_Clean_Arch.Infrastructure.Configurations;
using ECommerce_Clean_Arch.Infrastructure.Services;

using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using StackExchange.Redis;

namespace ECommerce_Clean_Arch.Infrastructure.Persistence.Repositories;

public sealed class CartRepository : ICartRepository
{
    private readonly IDatabase _database;
    private readonly CartKeyResolver _keyResolver;
    private readonly TimeSpan _ttl;

    public CartRepository(
        IConnectionMultiplexer connectionMultiplexer,
        CartKeyResolver keyResolver,
        IOptions<CartTtlConfig> config,
        IUser user
    )
    {
        _database = connectionMultiplexer.GetDatabase();
        _keyResolver = keyResolver;
        _ttl = user.Id == null
            ? TimeSpan.FromDays(config.Value.GuestTtlDays)
            : TimeSpan.FromDays(config.Value.UserTtlDays);
    }

    public async Task<Cart?> GetCartAsync()
    {
        var jsonData = await _database.StringGetAsync(_keyResolver.GetCartKey());
        if (jsonData.IsNullOrEmpty)
        {
            return null;
        }

        var cart = JsonConvert.DeserializeObject<Cart>(jsonData.ToString());
        if (cart is null) throw new RedisDeserializationException(nameof(Cart));
        return cart;
    }

    public async Task SetCartAsync(Cart cart)
    {
        var jsonData = JsonConvert.SerializeObject(cart);
        await _database.StringSetAsync(
            _keyResolver.GetCartKey(),
            jsonData,
            _ttl);
    }
}