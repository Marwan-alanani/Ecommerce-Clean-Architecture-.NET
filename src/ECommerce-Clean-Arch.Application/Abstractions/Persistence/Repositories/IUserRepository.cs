
namespace ECommerce_Clean_Arch.Application.Abstractions.Persistence.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
}