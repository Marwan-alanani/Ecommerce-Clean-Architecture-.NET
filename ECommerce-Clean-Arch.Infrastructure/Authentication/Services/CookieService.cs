using ECommerce_Clean_Arch.Application.Authentication.Services;
using ECommerce_Clean_Arch.Application.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ECommerce_Clean_Arch.Infrastructure.Authentication.Services;

public class CookieService : ICookieService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtConfig _jwtConfig;
    private readonly DateTime _utcNow;
    private const string RefreshTokenCookieName = "refreshToken";

    public CookieService(
        IHttpContextAccessor httpContextAccessor,
        IOptions<JwtConfig> jwtConfig,
        IDateTimeProvider dateTimeProvider
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _utcNow = dateTimeProvider.UtcNow;
        _jwtConfig = jwtConfig.Value;
    }

    public string? GetRefreshToken()
    {
        return _httpContextAccessor.HttpContext?.Request.Cookies[RefreshTokenCookieName];
    }

    public void SetRefreshToken(string refreshToken)
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Append(
            RefreshTokenCookieName,
            refreshToken,
            new CookieOptions
            {
                Expires = _utcNow.AddDays(_jwtConfig.RefreshTokenExpiryInDays),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/",
                IsEssential = true,
            });
    }

    public void ClearRefreshToken()
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete(RefreshTokenCookieName);
    }
}