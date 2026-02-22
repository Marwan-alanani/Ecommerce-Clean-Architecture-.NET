using ECommerce_Clean_Arch.Application.Persistence;
using ECommerce_Clean_Arch.Domain.Products;

namespace ECommerce_Clean_Arch.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProductRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<Product>().AddAsync(product, cancellationToken);
    }
}