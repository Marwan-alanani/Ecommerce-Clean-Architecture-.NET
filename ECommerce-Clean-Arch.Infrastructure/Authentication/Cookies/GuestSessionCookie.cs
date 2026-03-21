using Microsoft.AspNetCore.Http;

namespace ECommerce_Clean_Arch.Infrastructure.Authentication.Cookies;

public static class GuestSessionCookie
{
    public const string CookieName = "guest_session";

    public static CookieOptions Options(int expiryInDays, DateTime utcNow) => new()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Expires = utcNow.AddDays(expiryInDays)
    };

    public static CookieOptions DeletionOptions => new()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict
    };
}