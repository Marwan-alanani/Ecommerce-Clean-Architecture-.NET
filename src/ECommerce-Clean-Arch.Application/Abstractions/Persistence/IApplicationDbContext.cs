using ECommerce_Clean_Arch.Domain.ProductComments;

namespace ECommerce_Clean_Arch.Application.Abstractions.Persistence;

public interface IApplicationDbContext
{
    DbSet<Category> Categories { get; }
    DbSet<Product> Products { get; }
    DbSet<User> Users { get; }
    DbSet<Order> Orders { get; }
    DbSet<ProductComment> ProductComments{ get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}