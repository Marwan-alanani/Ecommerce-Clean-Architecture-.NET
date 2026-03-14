using ECommerce_Clean_Arch.Application.Products.Commands.Create;
using ECommerce_Clean_Arch.Application.Products.Commands.Deactivate;
using ECommerce_Clean_Arch.Application.Products.Commands.Update;
using ECommerce_Clean_Arch.Application.Products.Queries.GetAll;
using ECommerce_Clean_Arch.Application.Products.Queries.GetById;
using ECommerce_Clean_Arch.Domain.Common.Security;
using ECommerce_Clean_Arch.Presentation.Attributes;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Clean_Arch.Presentation.Controllers;

[Route("products")]
public class ProductController : ApiController
{
    private readonly ISender _sender;

    public ProductController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [HasPermission(Permissions.Products.Write)]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
        var result = await _sender.Send(command);

        if (result.IsSuccess)
            return Created($"/products/{result.Value}", result.Value);

        return Problem(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var query = new GetProductById(id);
        var result = await _sender.Send(query);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return Problem(result.Error);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllProductsQuery query)
    {
        var result = await _sender.Send(query);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return Problem(result.Error);
    }

    [HttpPatch]
    [HasPermission(Permissions.Products.Edit)]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommand command)
    {
        var result = await _sender.Send(command);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return Problem(result.Error);
    }

    [HttpDelete("deactivate/{id}")]
    [HasPermission(Permissions.Products.Delete)]
    public async Task<IActionResult> Deactivate([FromRoute] Guid id)
    {
        var command = new DeactivateProductCommand(id);
        var result = await _sender.Send(command);
        if (result.IsSuccess)
        {
            return NoContent();
        }

        return Problem(result.Error);
    }
}