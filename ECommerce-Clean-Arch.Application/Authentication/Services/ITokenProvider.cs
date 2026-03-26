
namespace ECommerce_Clean_Arch.Application.Authentication.Services;

public interface ITokenProvider
{
    Task<string> GenerateAccessTokenAsync(User user);
    string GenerateOpaqueToken(int count);
    string HashOpaqueToken(string token);
}