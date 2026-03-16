namespace ECommerce_Clean_Arch.Application.Authentication.Services;

public interface ICookieService
{
    string? GetRefreshToken();
    void SetRefreshToken(string refreshToken, DateTime expiresOnUtc);
    void ClearRefreshToken();
}