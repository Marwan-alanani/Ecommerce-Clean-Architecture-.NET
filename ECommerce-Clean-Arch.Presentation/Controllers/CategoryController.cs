using ECommerce_Clean_Arch.Application.Categories.Commands.Create;
using ECommerce_Clean_Arch.Application.Categories.Commands.Deactivate;
using ECommerce_Clean_Arch.Application.Categories.Commands.Update;
using ECommerce_Clean_Arch.Domain.Categories.ValueObjects;
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

    public sealed record UpdateCategoryRequest(string Name);

    [HttpPatch("{id}")]
    [HasPermission(Permissions.Categories.Update)]
    public async Task<IActionResult> UpdateCategory(
        [FromRoute] Guid id,
        [FromBody] UpdateCategoryRequest
            request
    )
    {
        var categoryId = CategoryId.FromValue(id);
        var command = new UpdateCategoryCommand(categoryId, request.Name);
        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        return Ok(new { message = "category updated successfully" });
    }

    [HttpDelete("deactivate/{id}")]
    [HasPermission(Permissions.Categories.Delete)]
    public async Task<IActionResult> DeactivateCategory([FromRoute] Guid id)
    {
        var command = new DeactivateCategoryCommand(CategoryId.FromValue(id));
        var result = await _sender.Send(command);
        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        return Ok(new { message = "category deactivated successfully" });
    }
}