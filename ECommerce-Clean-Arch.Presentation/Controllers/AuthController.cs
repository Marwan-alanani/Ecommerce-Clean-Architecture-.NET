using System.Security.Claims;

using ECommerce_Clean_Arch.Application.Authentication.Commands.Logout;
using ECommerce_Clean_Arch.Application.Authentication.Commands.LogoutAllSessions;
using ECommerce_Clean_Arch.Application.Authentication.Commands.RegisterUser;
using ECommerce_Clean_Arch.Application.Authentication.Commands.RotateTokens;
using ECommerce_Clean_Arch.Application.Authentication.Queries.Login;
using ECommerce_Clean_Arch.Domain.Errors.Token;
using ECommerce_Clean_Arch.Domain.RefreshTokens;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Presentation.Controllers;

[Route("/auth")]
public class AuthController : ApiController
{
    private readonly ISender _sender;


    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterCommand command,
        CancellationToken cancellationToken
    )
    {
        var result = await _sender.Send(command, cancellationToken);
        if (result.IsSuccess)
        {
            return Ok(new { value = result.Value });
        }

        return Problem(result.Error);
    }

    public record LoginRequest(string Email, string Password);

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var query = new LoginQuery(
            request.Email,
            request.Password,
            HttpContext.Request.Headers.UserAgent.ToString(),
            Request.HttpContext.Connection.RemoteIpAddress?.ToString()
        );
        var result = await _sender.Send(query);
        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        var tokens = result.Value;
        // Send token in HttpOnly cookie
        SendRefreshTokenInCookies(tokens.RefreshToken.Token, tokens.RefreshToken.Expiration);
        return Ok(new { token = tokens.AccessToken });
    }

    public sealed record RotateTokensRequest(string Token);

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var token = Request.Cookies[RefreshToken.CookieName];
        if (token is null)
        {
            var error = Error.NotFound(new MissingTokenCookie());
            return Problem(error);
        }

        var command = new RotateTokensCommand(
            token,
            HttpContext.Request.Headers.UserAgent.ToString(),
            Request.HttpContext.Connection.RemoteIpAddress?.ToString()
        );
        var result = await _sender.Send(command);
        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        var tokens = result.Value;
        // Send token in HttpOnly cookie
        SendRefreshTokenInCookies(tokens.RefreshToken.Token, tokens.RefreshToken.Expiration);
        return Ok(new { token = tokens.AccessToken });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var token = Request.Cookies[RefreshToken.CookieName];
        if (token is null) // token doesn't exists in browser
        {
            return Ok(new { message = "Logged out successfully" });
        }

        var command = new LogoutCommand(token);
        var result = await _sender.Send(command);
        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        Response.Cookies.Delete(RefreshToken.CookieName);

        return Ok(new { message = "Logged out successfully" });
    }

    [Authorize]
    [HttpPost("logout-all")]
    public async Task<IActionResult> LogoutAllSessions()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
        {
            return Unauthorized(new { message = "User Id not found in token" });
        }

        var command = new LogoutAllSessionsCommand(userId);
        var result = await _sender.Send(command);
        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        Response.Cookies.Delete(RefreshToken.CookieName);
        return Ok(new { message = "Logged out of all sessions successfully" });
    }

    private void SendRefreshTokenInCookies(string token, DateTime expiration)
    {
        Response.Cookies.Append(
            RefreshToken.CookieName,
            token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = expiration
            });
    }
}