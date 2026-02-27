using ECommerce_Clean_Arch.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Clean_Arch.Application.Persistence;

public interface IApplicationDbContext
{
    DbSet<Product> Products { get; }
}