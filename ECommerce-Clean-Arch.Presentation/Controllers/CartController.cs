using ECommerce_Clean_Arch.Application.Carts.Commands.AddCartItem;
using ECommerce_Clean_Arch.Application.Carts.Commands.Create;
using ECommerce_Clean_Arch.Application.Carts.Queries.GetUserCart;
using ECommerce_Clean_Arch.Domain.Products.ValueObjects;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ECommerce_Clean_Arch.Presentation.Controllers;

[Route("carts")]
public sealed class CartController : ApiController
{
    private readonly ISender _sender;

    public CartController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateCart()
    {
        var command = new CreateCartCommand();
        var result = await _sender.Send(command);
        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMyCart()
    {
        var query = new GetUserCartQuery();
        var result = await _sender.Send(query);
        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        return Ok(result.Value);
    }

    public sealed record AddCartItemRequest(Guid ProductId, int Quantity);

    [HttpPost("items")]
    [Authorize]
    public async Task<IActionResult> AddCartItem([FromBody] AddCartItemRequest request)
    {
        var command = new AddCartItemCommand(
            ProductId.FromValue(request.ProductId),
            request.Quantity
        );
        var result = await _sender.Send(command);
        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        return Ok(result.Value);
    }
}