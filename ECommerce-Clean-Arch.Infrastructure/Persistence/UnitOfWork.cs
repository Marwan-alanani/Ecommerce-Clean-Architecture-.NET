using ECommerce_Clean_Arch.Application.Persistence;
using ECommerce_Clean_Arch.Application.Services;
using ECommerce_Clean_Arch.Domain.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Clean_Arch.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UnitOfWork(ApplicationDbContext applicationDbContext, IDateTimeProvider dateTimeProvider)
    {
        _applicationDbContext = applicationDbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = _applicationDbContext
            .ChangeTracker
            .Entries<IAuditable>();

        var now = _dateTimeProvider.UtcNow;
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.UpdatedAt = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
            }
        }

        return await _applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}