using ECommerce_Clean_Arch.Application.Orders.Queries.GetById;
using ECommerce_Clean_Arch.Application.Orders.Queries.GetPage;
using ECommerce_Clean_Arch.Domain.Orders.ValueObjects;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Clean_Arch.Presentation.Controllers;

[Route("orders")]
public class OrderController : ApiController
{
    private readonly ISender _sender;

    public OrderController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var orderId = OrderId.FromValue(id);
        var query = new GetOrderByIdQuery(orderId);
        var result = await _sender.Send(query, cancellationToken);
        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetByPage(
        [FromQuery] GetOrdersPageQuery query,
        CancellationToken cancellationToken
    )
    {
        var result = await _sender.Send(query, cancellationToken);
        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        return Ok(result.Value);
    }
}