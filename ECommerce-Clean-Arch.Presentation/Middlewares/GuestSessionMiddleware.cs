using ECommerce_Clean_Arch.Application.Abstractions.Services;
using ECommerce_Clean_Arch.Infrastructure.Authentication.Cookies;

namespace ECommerce_Clean_Arch.Presentation.Middlewares;

public class GuestSessionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ICookieService cookieService)
    {
        // authenticated do nothing
        if (context.User.Identity?.IsAuthenticated == true)
        {
            await next(context);
            return;
        }

        // User is not authenticated — ensure guest cookie exists
        var guestSessionId = context.Request.Cookies[GuestSessionCookie.CookieName];
        if (string.IsNullOrEmpty(guestSessionId))
        {
            var guestId = cookieService.SetGuestSessionCookie();
            context.Items[GuestSessionCookie.CookieName] = guestId;
        }
        else
        {
            context.Items[GuestSessionCookie.CookieName] = guestSessionId;
        }

        await next(context);
    }
}