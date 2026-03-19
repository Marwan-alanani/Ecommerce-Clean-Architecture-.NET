using ECommerce_Clean_Arch.Domain.Users;

namespace ECommerce_Clean_Arch.Application.Authentication.Services;

public interface ITokenProvider
{
    Task<string> GenerateAccessTokenAsync(User user);
    string GenerateOpaqueToken();
    string HashOpaqueToken(string token);
}