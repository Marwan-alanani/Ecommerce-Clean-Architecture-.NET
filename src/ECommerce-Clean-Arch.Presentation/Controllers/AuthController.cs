using ECommerce_Clean_Arch.Application.Authentication.Commands.ChangePassword;
using ECommerce_Clean_Arch.Application.Authentication.Commands.Logout;
using ECommerce_Clean_Arch.Application.Authentication.Commands.LogoutAllSessions;
using ECommerce_Clean_Arch.Application.Authentication.Commands.RefreshTokens;
using ECommerce_Clean_Arch.Application.Authentication.Commands.RegisterUser;
using ECommerce_Clean_Arch.Application.Authentication.Queries.Login;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


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

        return Ok(new { token = result.Value });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var command = new RefreshTokensCommand();
        var result = await _sender.Send(command);
        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        return Ok(new { token = result.Value });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var command = new LogoutCommand();
        var result = await _sender.Send(command);
        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        return Ok(new { message = "Logged out successfully" });
    }

    [Authorize]
    [HttpPost("logout-all")]
    public async Task<IActionResult> LogoutAllSessions()
    {
        var command = new LogoutAllSessionsCommand();
        var result = await _sender.Send(command);
        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        return Ok(new { message = "Logged out of all sessions successfully" });
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
    {
        var result = await _sender.Send(command);
        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        return Ok(new { message = "Password changed successfully" });
    }
}