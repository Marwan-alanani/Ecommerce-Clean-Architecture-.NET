using ECommerce_Clean_Arch.Application.Authentication.Commands.RegisterUser;
using ECommerce_Clean_Arch.Application.Authentication.Queries;
using ECommerce_Clean_Arch.Contracts.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Clean_Arch.Presentation.Controllers;

[Route("/auth")]
public class AuthController : ApiController
{
    private readonly ISender _mediator;

    public AuthController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new RegisterCommand(
            request.Username,
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName);

        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
        {
            var authResponse = new AuthenticationResponse(
                result.Value.User.Id,
                result.Value.User.UserName!,
                result.Value.User.Email!,
                result.Value.User.FirstName,
                result.Value.User.LastName,
                result.Value.Token);

            return Ok(authResponse);
        }

        return Problem(result.Error);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]LoginRequest request)
    {
        var query = new LoginQuery(request.Email, request.Password);
        var result = await _mediator.Send(query);
        if (result.IsSuccess)
        {
            var authResponse = new AuthenticationResponse(
                result.Value.User.Id,
                result.Value.User.UserName!,
                result.Value.User.Email!,
                result.Value.User.FirstName,
                result.Value.User.LastName,
                result.Value.Token);

            return Ok(authResponse);
        }

        return Problem(result.Error);
    }
}