using ECommerce_Clean_Arch.Application.Categories.Create;
using ECommerce_Clean_Arch.Application.Categories.Update;
using ECommerce_Clean_Arch.Domain.Common.Security;
using ECommerce_Clean_Arch.Presentation.Attributes;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Clean_Arch.Presentation.Controllers;

[Route("categories")]
public sealed class CategoryController : ApiController
{
    private readonly ISender _sender;

    public CategoryController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [HasPermission(Permissions.Categories.Create)]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
    {
        var result = await _sender.Send(command);
        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        return Created("/categories/id", result.Value);
    }

    [HttpPatch]
    [HasPermission(Permissions.Categories.Update)]
    public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryCommand command)
    {
        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        return Ok(new { message = "category updated successfully" });
    }
}