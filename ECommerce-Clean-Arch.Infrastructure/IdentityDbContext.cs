using ECommerce_Clean_Arch.Domain.Common.Interfaces;
using ECommerce_Clean_Arch.Domain.Users;
using ECommerce_Clean_Arch.Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Clean_Arch.Infrastructure;

public class IdentityDbContext(DbContextOptions<IdentityDbContext> options) :
    IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserConfigurations());
        base.OnModelCreating(builder);
    }

    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = new()
    )
    {
        var now = DateTime.UtcNow;
        var addedEntities = ChangeTracker.Entries<IAuditable>()
            .Where(e => e.State == EntityState.Added)
            .Select(e => e.Entity);
        foreach (var entity in addedEntities)
        {
            entity.UpdatedAt = now;
            entity.CreatedAt = now;
        }

        var updatedEntities = ChangeTracker.Entries<IAuditable>()
            .Where(e => e.State == EntityState.Modified)
            .Select(e => e.Entity);

        foreach (var entity in updatedEntities)
            entity.UpdatedAt = now;

        return base.SaveChangesAsync(cancellationToken);
    }
}