using ECommerce_Clean_Arch.Domain.Categories;
using ECommerce_Clean_Arch.Domain.Categories.ValueObjects;

namespace ECommerce_Clean_Arch.Application.Persistence.Repositories;

public interface ICategoryRepository
{
    Task<bool> CategoryExists(string categoryName, CancellationToken cancellationToken = default);
    Task<bool> CategoryExists(CategoryId id, CancellationToken cancellationToken = default);
    Task AddAsync(Category category, CancellationToken cancellationToken = default);
    Task<Category?> GetCategoryAsync(CategoryId id, CancellationToken cancellationToken = default);
}