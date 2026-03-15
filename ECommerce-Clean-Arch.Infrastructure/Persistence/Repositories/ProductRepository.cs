using ECommerce_Clean_Arch.Application.Persistence.Repositories;
using ECommerce_Clean_Arch.Domain.Categories.ValueObjects;
using ECommerce_Clean_Arch.Domain.Products;
using ECommerce_Clean_Arch.Domain.Products.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce_Clean_Arch.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private const int BatchSize = 500;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(ApplicationDbContext context, ILogger<ProductRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    // read only without tracking
    public IQueryable<Product> Products => _context.Set<Product>().AsNoTracking();

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default) =>
        await _context.Set<Product>().AddAsync(product, cancellationToken);

    public async Task<bool> NameExists(string name, CancellationToken cancellationToken = default) =>
        await _context.Set<Product>().AnyAsync(p => p.Name == name, cancellationToken);

    public async Task<Product?> GetByIdAsync(ProductId id, CancellationToken cancellationToken = default)
    {
        var product = await _context.Set<Product>()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        return product;
    }

    public async Task DeactivateByCategoryIdAsync(
        CategoryId categoryId,
        CancellationToken cancellationToken = default
    )
    {
        var processed = 0;
        while (true)
        {
            var products = await _context.Set<Product>()
                .Where(p => p.CategoryId == categoryId && p.IsActive)
                .OrderBy(p => p.Id)
                .Skip(processed)
                .Take(BatchSize)
                .ToListAsync(cancellationToken);
            if (!products.Any())
            {
                break;
            }

            foreach (var product in products) product.Deactivate();
            await _context.SaveChangesAsync(cancellationToken);
            _context.ChangeTracker.Clear();
            processed += products.Count;
        }

        _logger.LogInformation($"Deactivated a total of {processed} products.");
    }

    public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<Product>().ToListAsync(cancellationToken);
    }
}