using ECommerce_Clean_Arch.Domain.Users;

namespace ECommerce_Clean_Arch.Application.Authentication;

public interface IJwtTokenGenerator
{
    string Generate(User user);
}