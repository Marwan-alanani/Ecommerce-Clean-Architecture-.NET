using ECommerce_Clean_Arch.Application.Persistence.Repositories;
using ECommerce_Clean_Arch.Domain.Categories;
using ECommerce_Clean_Arch.Domain.Categories.ValueObjects;

using Microsoft.EntityFrameworkCore;

namespace ECommerce_Clean_Arch.Infrastructure.Persistence.Repositories;

public sealed class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CategoryExists(
        string categoryName,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Set<Category>().AnyAsync(c => c.Name == categoryName, cancellationToken);
    }

    public async Task<bool> CategoryExists(CategoryId id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Category>().AnyAsync(c => c.Id == id, cancellationToken);
    }

    public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        await _context.Set<Category>().AddAsync(category, cancellationToken);
    }
}