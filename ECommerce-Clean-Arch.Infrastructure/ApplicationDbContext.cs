using ECommerce_Clean_Arch.Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Clean_Arch.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
    DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // applicationDbContext should not have the user entity
        modelBuilder
            .Ignore<User>()
            .ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}