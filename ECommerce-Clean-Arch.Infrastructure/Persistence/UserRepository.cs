using ECommerce_Clean_Arch.Application.Persistence;
using ECommerce_Clean_Arch.Domain.Users;

namespace ECommerce_Clean_Arch.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private readonly List<User> _users = new List<User>();

    public void Save(User user)
    {
        _users.Add(user);
    }

    public User GetUser(Guid id)
    {
        return _users.FirstOrDefault(u => u.Id == id)!;
    }
}