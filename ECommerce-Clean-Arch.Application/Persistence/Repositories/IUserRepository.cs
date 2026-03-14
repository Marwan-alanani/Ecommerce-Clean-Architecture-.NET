using ECommerce_Clean_Arch.Domain.Users;

namespace ECommerce_Clean_Arch.Application.Persistence.Repositories;

public interface IUserRepository
{
    void Update(User user);
    Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
}