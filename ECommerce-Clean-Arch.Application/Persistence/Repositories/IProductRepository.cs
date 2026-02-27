using ECommerce_Clean_Arch.Domain.Products;
using ECommerce_Clean_Arch.Domain.Products.ValueObjects;

namespace ECommerce_Clean_Arch.Application.Persistence.Repositories;

public interface IProductRepository
{
    Task AddAsync(Product product, CancellationToken cancellationToken = default);
    Task<bool> NameExists(string name, CancellationToken cancellationToken = default);
    Task<Product?> GetByIdAsync(ProductId id, CancellationToken cancellationToken = default);
    Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default);
}