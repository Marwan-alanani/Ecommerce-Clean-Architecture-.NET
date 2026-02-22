using ECommerce_Clean_Arch.Domain.Products;

namespace ECommerce_Clean_Arch.Application.Persistence;

public interface IProductRepository
{
    Task AddAsync(Product product, CancellationToken cancellationToken = default);
}