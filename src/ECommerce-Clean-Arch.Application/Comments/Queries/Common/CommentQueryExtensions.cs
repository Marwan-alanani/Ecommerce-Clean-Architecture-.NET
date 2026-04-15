using ECommerce_Clean_Arch.Domain.ProductComments;

namespace ECommerce_Clean_Arch.Application.Comments.Queries.Common;

public static class CommentQueryExtensions
{
    public static IQueryable<ProductCommentDto> ToDto(this IQueryable<ProductComment> comments)
    {
        return comments.Select(c => new ProductCommentDto(
            c.UserId,
            c.UserName,
            c.ProductId.Value,
            c.Content,
            c.CreatedAt)
        );
    }
}