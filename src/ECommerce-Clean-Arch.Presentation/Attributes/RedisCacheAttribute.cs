using System.Text;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Newtonsoft.Json;

using StackExchange.Redis;

namespace ECommerce_Clean_Arch.Presentation.Attributes;

public sealed class RedisCacheAttribute(int durationInSeconds = 90) : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next
    )
    {
        var database = context.HttpContext.RequestServices
            .GetRequiredService<IConnectionMultiplexer>()
            .GetDatabase();
        var cacheKey = GetKey(context.HttpContext.Request);
        var cacheValue = await database.StringGetAsync(cacheKey);
        if (!cacheValue.IsNullOrEmpty)
        {
            context.Result = new ContentResult
            {
                Content = cacheValue,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
            return;
        }

        var executedContext = await next();
        if (executedContext.Result is OkObjectResult result)
        {
            await database.StringSetAsync(
                cacheKey,
                JsonConvert.SerializeObject(result.Value!),
                TimeSpan.FromSeconds(durationInSeconds)
            );
        }
    }

    private string GetKey(HttpRequest request)
    {
        var key = new StringBuilder();
        key.Append(request.Path.Value);
        if (request.Query.Any()) key.Append('?');

        foreach (var kv in request.Query.OrderBy(kv => kv.Key))
            key.Append($"{kv.Key}={kv.Value}&");

        return key.ToString().TrimEnd('&');
    }
}