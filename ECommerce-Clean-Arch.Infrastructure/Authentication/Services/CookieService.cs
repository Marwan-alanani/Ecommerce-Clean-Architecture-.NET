using ECommerce_Clean_Arch.Application.Authentication.Services;
using ECommerce_Clean_Arch.Domain.RefreshTokens;

using Microsoft.AspNetCore.Http;

namespace ECommerce_Clean_Arch.Infrastructure.Authentication.Services;

public class CookieService : ICookieService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetRefreshToken()
    {
        return _httpContextAccessor.HttpContext?.Request.Cookies[RefreshToken.CookieName];
    }

    public void SetRefreshToken(string refreshToken, DateTime expiresOnUtc)
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Append(
            RefreshToken.CookieName,
            refreshToken,
            new CookieOptions
            {
                Expires = expiresOnUtc,
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/",
                IsEssential = true,
            });
    }

    public void ClearRefreshToken()
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete(RefreshToken.CookieName);
    }
}