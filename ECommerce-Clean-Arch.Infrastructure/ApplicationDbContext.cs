using ECommerce_Clean_Arch.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Clean_Arch.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
    DbContext(options)
{
    public const string ConnectionStringName = "AppDb";

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // applicationDbContext should not have the user entity
        modelBuilder
            .ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly)
            .Ignore<User>();
        base.OnModelCreating(modelBuilder);
    }

}