using ECommerce_Clean_Arch.Application.Users.Commands.RegisterUser;
using ECommerce_Clean_Arch.Contracts.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Clean_Arch.Presentation.Controllers;

[ApiController]
[Route("/users")]
public class UserController : ControllerBase
{
    private readonly ISender _mediator;

    public UserController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(RegisterUserRequest request)
    {
        var command = new RegisterUserCommand(
            request.Username,
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName);

        var result = await _mediator.Send(command);
        return Ok(result);
    }
}