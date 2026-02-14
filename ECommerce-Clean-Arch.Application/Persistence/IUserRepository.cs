using ECommerce_Clean_Arch.Domain.Users;

namespace ECommerce_Clean_Arch.Application.Persistence;

public interface IUserRepository
{
    void Save(User user);
    User GetUser(Guid id);
}