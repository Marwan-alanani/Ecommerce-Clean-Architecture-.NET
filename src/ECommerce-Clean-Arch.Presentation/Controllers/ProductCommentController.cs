using ECommerce_Clean_Arch.Application.Comments.Commands.Add;
using ECommerce_Clean_Arch.Application.Comments.Queries.GetPage;
using ECommerce_Clean_Arch.Domain.Products.ValueObjects;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce_Clean_Arch.Presentation.Controllers;

[Route("comments")]
public class ProductCommentController : ApiController
{
    private readonly ISender _sender;

    public ProductCommentController(ISender sender)
    {
        _sender = sender;
    }

    public sealed record AddCommentRequest(string Content);

    [HttpPost("{productId:guid}")]
    [Authorize]
    public async Task<IActionResult> CreateComment(
        [FromRoute] Guid productId,
        [FromBody] AddCommentRequest request
    )
    {
        var command = new AddCommentCommand(request.Content, ProductId.FromValue(productId));
        var result = await _sender.Send(command);
        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        return Ok();
    }

    [HttpGet("{productId:guid}")]
    public async Task<IActionResult> GetCommentPage([FromRoute] Guid productId, [FromQuery] int? pageNo)
    {
        var query = new GetCommentsPageQuery()
        {
            ProductId = ProductId.FromValue(productId), PageNo = pageNo
        };

        var result = await _sender.Send(query);

        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        return Ok(result.Value);

    }
}