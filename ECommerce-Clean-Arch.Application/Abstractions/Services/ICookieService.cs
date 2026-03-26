namespace ECommerce_Clean_Arch.Application.Abstractions.Services;

public interface ICookieService
{
    string? GetRefreshToken();
    void SetRefreshToken(string refreshToken);
    void ClearRefreshToken();
    void ClearGuestSession();
    public string SetGuestSessionCookie();
    public string? GetGuestSessionId();
}