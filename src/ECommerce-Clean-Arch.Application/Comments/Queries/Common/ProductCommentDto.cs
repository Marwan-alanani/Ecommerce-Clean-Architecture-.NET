namespace ECommerce_Clean_Arch.Application.Comments.Queries.Common;

public sealed record ProductCommentDto(
    Guid UserId,
    string UserName,
    Guid ProductId,
    string Content,
    DateTime CreatedAt
);