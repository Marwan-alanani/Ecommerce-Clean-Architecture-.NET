using ECommerce_Clean_Arch.Application.Carts.Commands.SetItem;
using ECommerce_Clean_Arch.Domain.Products.ValueObjects;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Clean_Arch.Presentation.Controllers;

[Route("carts")]
public class CartController : ApiController
{
    private readonly ISender _sender;

    public CartController(ISender sender)
    {
        _sender = sender;
    }

    public sealed record SetItemInCartRequest(Guid ProductId, int Quantity);

    [HttpPost("items")]
    public async Task<IActionResult> SetItem([FromBody] SetItemInCartRequest request)
    {
        var command = new SetItemInCartCommand(
            ProductId.FromValue(request.ProductId),
            request.Quantity
        );
        var result = await _sender.Send(command);
        if (result.IsFailure) return Problem(result.Error);
        return Ok(new { message = "item added to cart successfully" });
    }
}