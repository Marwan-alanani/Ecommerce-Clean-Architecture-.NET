using ECommerce_Clean_Arch.Application.Products.Commands.CreateProduct;
using ECommerce_Clean_Arch.Contracts.Products;
using ECommerce_Clean_Arch.Domain.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Models;

namespace ECommerce_Clean_Arch.Presentation.Controllers;

[Route("/products")]
public class ProductController : ApiController
{
    private readonly ISender _mediator;

    public ProductController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
    {
        var command = new CreateProductCommand(
            request.Name,
            request.Description,
            new Money(Enum.Parse<Currency>(request.Currency), request.Amount),
            request.PictureUrl);
        var result = await _mediator.Send(command);
        if (result.IsSuccess)
        {
            var product = result.Value;
            var response = new ProductResponse(
                product.Name,
                product.Description,
                product.Price.Amount,
                product.Price.Currency.ToString(),
                product.CreatedAt,
                product.UpdatedAt);
            return Ok(response);
        }
        return Problem();
    }
}