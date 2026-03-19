using ECommerce_Clean_Arch.Application.Common.Constants;

namespace ECommerce_Clean_Arch.Presentation.Middlewares;

public class GuestSessionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // If user is authenticated, kill the guest cookie if it exists
        if (context.User.Identity?.IsAuthenticated == true)
        {
            if (context.Request.Cookies.ContainsKey(CookieNames.GuestSession))
            {
                context.Response.Cookies.Delete(CookieNames.GuestSession);
            }

            await next(context);
            return;
        }

        // User is not authenticated — ensure guest cookie exists
        if (!context.Request.Cookies.ContainsKey(CookieNames.GuestSession))
        {
            var guestId = Guid.NewGuid().ToString();
            context.Response.Cookies.Append(
                CookieNames.GuestSession,
                guestId,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });
            context.Items[CookieNames.GuestSession] = guestId;
        }
        else
        {
            context.Items[CookieNames.GuestSession] = context.Request.Cookies[CookieNames.GuestSession];
        }

        await next(context);
    }
}