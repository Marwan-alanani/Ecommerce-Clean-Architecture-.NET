using ECommerce_Clean_Arch.Application.Persistence.Repositories;
using ECommerce_Clean_Arch.Domain.Products;
using ECommerce_Clean_Arch.Domain.Products.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Clean_Arch.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProductRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // read only without tracking
    public IQueryable<Product> Products => _dbContext.Set<Product>().AsNoTracking();

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default) =>
        await _dbContext.Set<Product>().AddAsync(product, cancellationToken);

    public async Task<bool> NameExists(string name, CancellationToken cancellationToken = default) =>
        await _dbContext.Set<Product>().AnyAsync(p => p.Name == name, cancellationToken);

    public async Task<Product?> GetByIdAsync(ProductId id, CancellationToken cancellationToken = default)
    {
        var product = await _dbContext.Set<Product>()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        return product;
    }

    public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Product>().ToListAsync(cancellationToken);
    }
}