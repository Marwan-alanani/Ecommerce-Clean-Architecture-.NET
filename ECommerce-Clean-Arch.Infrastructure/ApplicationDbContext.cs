using ECommerce_Clean_Arch.Application.Persistence;
using ECommerce_Clean_Arch.Domain.Products;
using ECommerce_Clean_Arch.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Clean_Arch.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
    DbContext(options), IApplicationDbContext
{
    public const string ConnectionStringName = "AppDb";
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // applicationDbContext should not have the user entity
        modelBuilder
            .ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly)
            .Ignore<User>();
        base.OnModelCreating(modelBuilder);
    }
}