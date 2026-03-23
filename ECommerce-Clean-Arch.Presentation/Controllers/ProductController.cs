using ECommerce_Clean_Arch.Application.Products.Commands.Create;
using ECommerce_Clean_Arch.Application.Products.Commands.Deactivate;
using ECommerce_Clean_Arch.Application.Products.Commands.Update;
using ECommerce_Clean_Arch.Application.Products.Queries.GetAll;
using ECommerce_Clean_Arch.Application.Products.Queries.GetById;
using ECommerce_Clean_Arch.Domain.Categories.ValueObjects;
using ECommerce_Clean_Arch.Domain.Common.Security;
using ECommerce_Clean_Arch.Domain.Products.ValueObjects;
using ECommerce_Clean_Arch.Presentation.Attributes;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using SharedKernel.Models;

namespace ECommerce_Clean_Arch.Presentation.Controllers;

[Route("products")]
public class ProductController : ApiController
{
    private readonly ISender _sender;

    public ProductController(ISender sender)
    {
        _sender = sender;
    }

    public sealed record CreateProductRequest(
        string Name,
        string Description,
        MoneyFlat Price,
        string PictureUrl,
        Guid CategoryId
    );

    [HttpPost]
    [HasPermission(Permissions.Products.Write)]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new CreateProductCommand(
            request.Name,
            request.Description,
            request.Price,
            request.PictureUrl,
            CategoryId.FromValue(request.CategoryId)
        );
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsSuccess)
            return Created($"/products/{result.Value}", result.Value);

        return Problem(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetProductByIdQuery(ProductId.FromValue(id));
        var result = await _sender.Send(query, cancellationToken);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return Problem(result.Error);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllProductsQuery query,
        CancellationToken cancellationToken
    )
    {
        var result = await _sender.Send(query, cancellationToken);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return Problem(result.Error);
    }

    public sealed record UpdateProductRequest(
        Guid Id,
        string? Name,
        string? Description,
        MoneyFlat? Price,
        Guid? CategoryId
    );

    [HttpPatch]
    [HasPermission(Permissions.Products.Edit)]
    public async Task<IActionResult> UpdateProduct(
        [FromBody] UpdateProductRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new UpdateProductCommand(
            ProductId.FromValue(request.Id),
            request.Name,
            request.Description,
            request.Price,
            request.CategoryId == null ? null : CategoryId.FromValue(request.CategoryId.Value)
        );
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return Problem(result.Error);
    }

    [HttpDelete("deactivate/{id}")]
    [HasPermission(Permissions.Products.Delete)]
    public async Task<IActionResult> Deactivate([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeactivateProductCommand(ProductId.FromValue(id));
        var result = await _sender.Send(command, cancellationToken);
        if (result.IsSuccess)
        {
            return NoContent();
        }

        return Problem(result.Error);
    }
}