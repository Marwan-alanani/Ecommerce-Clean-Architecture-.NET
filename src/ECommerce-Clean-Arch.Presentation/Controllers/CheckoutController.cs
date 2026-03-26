using ECommerce_Clean_Arch.Application.Orders.Commands.Checkout;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Clean_Arch.Presentation.Controllers;

[Route("checkout")]
public class CheckoutController : ApiController
{
    private readonly ISender _sender;

    public CheckoutController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Checkout([FromBody] CheckoutCommand command)
    {
        var result = await _sender.Send(command);
        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        return Ok(result.Value);
    }
}