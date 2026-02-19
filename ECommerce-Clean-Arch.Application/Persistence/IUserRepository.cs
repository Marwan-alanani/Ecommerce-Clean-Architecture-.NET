using ECommerce_Clean_Arch.Domain.Users;
using ECommerce_Clean_Arch.Domain.Users.ValueObjects;

namespace ECommerce_Clean_Arch.Application.Persistence;

public interface IUserRepository
{
    void Save(User user);
    bool EmailExists(string email);
    User Get(UserId id);
}