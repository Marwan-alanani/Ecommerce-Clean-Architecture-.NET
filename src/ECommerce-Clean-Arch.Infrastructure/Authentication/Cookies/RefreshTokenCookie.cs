using Microsoft.AspNetCore.Http;

namespace ECommerce_Clean_Arch.Infrastructure.Authentication.Cookies;

public static class RefreshTokenCookie
{
    public const string CookieName = "refresh_token";

    public static CookieOptions Options(int expiryInDays, DateTime utcNow) => new()
    {
        Expires = utcNow.AddDays(expiryInDays),
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Path = "/",
        IsEssential = true,
    };

    public static CookieOptions DeletionOptions => new()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Path = "/",
        IsEssential = true,
    };
}