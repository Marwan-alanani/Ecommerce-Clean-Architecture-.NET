using ECommerce_Clean_Arch.Application.Services;
using ECommerce_Clean_Arch.Infrastructure.Authentication.Cookies;
using ECommerce_Clean_Arch.Infrastructure.Configurations;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ECommerce_Clean_Arch.Infrastructure.Services;

public class CookieService : ICookieService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtConfig _jwtConfig;
    private readonly IDateTimeProvider _dateTime;
    private readonly CartTtlConfig _cartTtlConfig;

    public CookieService(
        IHttpContextAccessor httpContextAccessor,
        IOptions<JwtConfig> jwtConfig,
        IDateTimeProvider dateTimeProvider,
        IOptions<CartTtlConfig> cartTtlConfig
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _cartTtlConfig = cartTtlConfig.Value;
        _dateTime = dateTimeProvider;
        _jwtConfig = jwtConfig.Value;
    }

    public string? GetRefreshToken()
    {
        return _httpContextAccessor.HttpContext?.Request.Cookies[RefreshTokenCookie.CookieName];
    }

    public void SetRefreshToken(string refreshToken)
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Append(
            RefreshTokenCookie.CookieName,
            refreshToken,
            RefreshTokenCookie.Options(_jwtConfig.RefreshTokenExpiryInDays, _dateTime.UtcNow)
        );
    }

    public void ClearRefreshToken()
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete(
            RefreshTokenCookie.CookieName,
            RefreshTokenCookie.DeletionOptions
        );
    }

    public void ClearGuestSession()
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete(
            GuestSessionCookie.CookieName,
            GuestSessionCookie.DeletionOptions);
        _httpContextAccessor.HttpContext?.Items.Remove(GuestSessionCookie.CookieName);
    }

    public string SetGuestSessionCookie()
    {
        var guestId = Guid.NewGuid().ToString();
        _httpContextAccessor.HttpContext?.Response.Cookies.Append(
            GuestSessionCookie.CookieName,
            guestId,
            GuestSessionCookie.Options(_cartTtlConfig.GuestTtlDays, _dateTime.UtcNow));
        return guestId;
    }

    public string? GetGuestSessionId()
    {
        return _httpContextAccessor.HttpContext?.Request.Cookies[GuestSessionCookie.CookieName];
    }
}