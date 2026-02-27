using ECommerce_Clean_Arch.Application.Products.Commands.CreateProduct;
using ECommerce_Clean_Arch.Application.Products.Queries.GetAll;
using ECommerce_Clean_Arch.Application.Products.Queries.GetProductById;
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
        var query = new GetProductByIdQuery(id);
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
}