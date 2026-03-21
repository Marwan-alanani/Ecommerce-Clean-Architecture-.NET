using ECommerce_Clean_Arch.Application.Abstractions.Persistence.Repositories;
using ECommerce_Clean_Arch.Application.Carts.Models;
using ECommerce_Clean_Arch.Application.Common.Interfaces;
using ECommerce_Clean_Arch.Domain.Errors.Common.Exceptions;
using ECommerce_Clean_Arch.Infrastructure.Configurations;

using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using StackExchange.Redis;

namespace ECommerce_Clean_Arch.Infrastructure.Persistence.Repositories;

public sealed class CartRepository : ICartRepository
{
    private readonly IDatabase _database;
    private readonly TimeSpan _ttl;

    public CartRepository(
        IConnectionMultiplexer connectionMultiplexer,
        IOptions<CartTtlConfig> config,
        IUser user
    )
    {
        _database = connectionMultiplexer.GetDatabase();
        _ttl = user.Id == null
            ? TimeSpan.FromDays(config.Value.GuestTtlDays)
            : TimeSpan.FromDays(config.Value.UserTtlDays);
    }

    public async Task<Cart?> GetCartAsync(string key)
    {
        var jsonData = await _database.StringGetAsync(key);
        if (jsonData.IsNullOrEmpty)
        {
            return null;
        }

        var cart = JsonConvert.DeserializeObject<Cart>(jsonData.ToString());
        if (cart is null) throw new RedisDeserializationException(nameof(Cart));
        return cart;
    }

    public async Task SetCartAsync(string key, Cart cart)
    {
        var jsonData = JsonConvert.SerializeObject(cart);
        await _database.StringSetAsync(
            key,
            jsonData,
            _ttl);
    }

    public async Task RemoveCartAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }

    public async Task MergeCartAsync(string guestKey, string userKey)
    {
        var guestCartJson = await _database.StringGetAsync(guestKey);
        if (guestCartJson.IsNullOrEmpty) return;

        var userCartJson = await _database.StringGetAsync(userKey);
        var transaction = _database.CreateTransaction();
        if (userCartJson.IsNullOrEmpty)
        {
            // just set user cart to guest cart data
            _ = transaction.StringSetAsync(
                userKey,
                guestCartJson,
                _ttl);
        }

        else
        {
            // merge both ... if item in guest only then add to user cart ... if item in both then
            // choose the item in guest cart (most recent)
            var guestCart = JsonConvert.DeserializeObject<Cart>(guestCartJson.ToString());
            if (guestCart is null) throw new RedisDeserializationException(nameof(Cart));
            var userCart = JsonConvert.DeserializeObject<Cart>(userCartJson.ToString());
            if (userCart is null) throw new RedisDeserializationException(nameof(Cart));
            foreach ((Guid key, CartItem value) in guestCart.Items)
            {
                userCart.SetCartItem(key, value);
            }

            var updatedUserCartJson = JsonConvert.SerializeObject(userCart);
            _ = transaction.StringSetAsync(
                userKey,
                updatedUserCartJson,
                _ttl);
        }

        // clear guest cart
        _ = transaction.KeyDeleteAsync(guestKey);
        await transaction.ExecuteAsync();
    }
}