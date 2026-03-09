using ECommerce_Clean_Arch.Domain.Users;

namespace ECommerce_Clean_Arch.Application.Authentication.Services;

public interface IJwtTokenService
{
    Task<string> Generate(User user);
}