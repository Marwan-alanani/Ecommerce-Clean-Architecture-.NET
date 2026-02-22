using ECommerce_Clean_Arch.Domain.Users;

namespace ECommerce_Clean_Arch.Application.Authentication.Interfaces;

public interface IJwtTokenGenerator
{
    string Generate(User user);
}