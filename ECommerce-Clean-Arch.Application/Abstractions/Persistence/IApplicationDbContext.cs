using ECommerce_Clean_Arch.Domain.Categories;
using ECommerce_Clean_Arch.Domain.Orders;
using ECommerce_Clean_Arch.Domain.Products;
using ECommerce_Clean_Arch.Domain.Users;

using Microsoft.EntityFrameworkCore;

namespace ECommerce_Clean_Arch.Application.Abstractions.Persistence;

public interface IApplicationDbContext
{
    DbSet<Category> Categories { get; }
    DbSet<Product> Products { get; }
    DbSet<User> Users { get; }
    DbSet<Order> Orders { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}