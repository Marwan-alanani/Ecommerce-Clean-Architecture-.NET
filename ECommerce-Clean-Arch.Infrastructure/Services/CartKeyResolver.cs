using ECommerce_Clean_Arch.Application.Common.Interfaces;
using ECommerce_Clean_Arch.Application.Services;
using ECommerce_Clean_Arch.Domain.Errors.Common.Exceptions;
using ECommerce_Clean_Arch.Infrastructure.Authentication.Cookies;

using Microsoft.AspNetCore.Http;

namespace ECommerce_Clean_Arch.Infrastructure.Services;

public sealed class CartKeyResolver : ICartKeyResolver
{
    private readonly IUser _user;
    private readonly IHttpContextAccessor _accessor;

    public CartKeyResolver(
        IUser user,
        IHttpContextAccessor accessor
    )
    {
        _user = user;
        _accessor = accessor;
    }

    public string GetCartKey()
    {
        var guestSessionId = _accessor.HttpContext?.Items[GuestSessionCookie.CookieName] as string;
        if (_user.Id.HasValue)
        {
            return GetUserKey(_user.Id.Value);
        }

        if (!string.IsNullOrEmpty(guestSessionId))
        {
            return GetGuestKey(guestSessionId);
        }

        throw new CannotComposeCartKey();
    }

    public string GetUserKey(Guid userId)
    {
        return "cart:user:" + userId;
    }

    public string GetGuestKey(string guestId)
    {
        return "cart:guest:" + guestId;
    }
}