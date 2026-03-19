using ECommerce_Clean_Arch.Application.Carts.Commands.Remove;
using ECommerce_Clean_Arch.Application.Carts.Commands.RemoveItem;
using ECommerce_Clean_Arch.Application.Carts.Commands.SetItem;
using ECommerce_Clean_Arch.Application.Carts.Queries.GetCart;
using ECommerce_Clean_Arch.Domain.Products.ValueObjects;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Clean_Arch.Presentation.Controllers;

[Route("cart")]
public class CartController : ApiController
{
    private readonly ISender _sender;

    public CartController(ISender sender)
    {
        _sender = sender;
    }

    public sealed record SetItemInCartRequest(Guid ProductId, int Quantity);

    [HttpPost("item")]
    public async Task<IActionResult> SetItem([FromBody] SetItemInCartRequest request)
    {
        var command = new SetItemInCartCommand(
            ProductId.FromValue(request.ProductId),
            request.Quantity
        );
        var result = await _sender.Send(command);
        if (result.IsFailure) return Problem(result.Error);
        return Ok(new { message = "item set in cart successfully" });
    }

    [HttpDelete("/items/{productId}")]
    public async Task<IActionResult> RemoveItem(Guid productId)
    {
        var command = new RemoveItemInCartCommand(productId);
        var result = await _sender.Send(command);

        if (result.IsFailure) return Problem(result.Error);
        return Ok(new { message = "Item remove from cart successfuly" });
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveCart()
    {
        var command = new RemoveCartCommand();
        var result = await _sender.Send(command);
        if (result.IsFailure) return Problem(result.Error);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var query = new GetCartQuery();
        var result = await _sender.Send(query);

        if (result.IsFailure) return Problem(result.Error);
        return Ok(result.Value);
    }
}