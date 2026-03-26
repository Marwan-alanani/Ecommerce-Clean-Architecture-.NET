using ECommerce_Clean_Arch.Application.Users.Commands.Deactivate;
using ECommerce_Clean_Arch.Domain.Common.Security;
using ECommerce_Clean_Arch.Presentation.Attributes;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Clean_Arch.Presentation.Controllers;

[Route("/users")]
public class UserController : ApiController
{
    private readonly ISender _sender;

    public UserController(ISender sender)
    {
        _sender = sender;
    }

    [HasPermission(Permissions.Users.Deactivate)]
    [HttpDelete("deactivate/{userId}")]
    public async Task<IActionResult> Deactivate([FromRoute] Guid userId)
    {
        var command = new DeactivateUserCommand(userId);
        var result = await _sender.Send(command);
        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        return NoContent();
    }
}