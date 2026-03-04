using ECommerce_Clean_Arch.Application.Persistence;

namespace ECommerce_Clean_Arch.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _applicationDbContext;

    public UnitOfWork(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
}