using ECommerce_Clean_Arch.Application.Common.Constants;
using ECommerce_Clean_Arch.Application.Common.Interfaces;

using Microsoft.AspNetCore.Http;

namespace ECommerce_Clean_Arch.Infrastructure.Services;

public sealed class CartKeyResolver
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private IUser _user;

    public CartKeyResolver(IHttpContextAccessor httpContextAccessor, IUser user)
    {
        _httpContextAccessor = httpContextAccessor;
        _user = user;
    }

    public string GetCartKey()
    {
        if (_httpContextAccessor.HttpContext!.Request.Cookies.ContainsKey(CookieNames.GuestSession))
        {
            return "cart:guest:" + (_httpContextAccessor.HttpContext.Request
                .Cookies[CookieNames.GuestSession])!;
        }

        if (_user.Id is null)
        {
            throw new Exception("user not found");
        }

        return "cart:" + _user.Id;
    }
}