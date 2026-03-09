using ECommerce_Clean_Arch.Domain.Users;

namespace ECommerce_Clean_Arch.Application.Authentication.Interfaces;

public interface IJwtTokenService
{
    Task<string> Generate(User user);
}