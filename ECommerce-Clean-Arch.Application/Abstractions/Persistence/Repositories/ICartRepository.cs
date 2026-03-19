using ECommerce_Clean_Arch.Application.Carts.Models;

namespace ECommerce_Clean_Arch.Application.Abstractions.Persistence.Repositories;

public interface ICartRepository
{
    Task<Cart?> GetCartAsync();
    Task SetCartAsync(Cart cart);
    Task RemoveCartAsync();
}