using ECommerce_Clean_Arch.Application.Users.Commands.RegisterUser;
using ECommerce_Clean_Arch.Contracts.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Clean_Arch.Presentation.Controllers;

[Route("/users")]
public class UserController : ApiController
{
    private readonly ISender _mediator;

    public UserController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
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
}