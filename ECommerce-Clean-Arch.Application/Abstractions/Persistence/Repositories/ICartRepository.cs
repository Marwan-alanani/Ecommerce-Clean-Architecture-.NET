using ECommerce_Clean_Arch.Application.Carts.Models;

namespace ECommerce_Clean_Arch.Application.Abstractions.Persistence.Repositories;

public interface ICartRepository
{
    Task<Cart?> GetCartAsync(string key);
    Task SetCartAsync(string key, Cart cart);
    Task RemoveCartAsync(string key);
    Task MergeCartAsync(string guestKey, string userKey);
}