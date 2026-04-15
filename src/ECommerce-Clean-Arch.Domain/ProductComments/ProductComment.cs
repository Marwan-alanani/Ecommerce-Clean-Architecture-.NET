using ECommerce_Clean_Arch.Domain.Common.Interfaces;
using ECommerce_Clean_Arch.Domain.Common.Models;
using ECommerce_Clean_Arch.Domain.ProductComments.ValueObjects;
using ECommerce_Clean_Arch.Domain.Products.ValueObjects;

namespace ECommerce_Clean_Arch.Domain.ProductComments;

public class ProductComment : AggregateRoot<ProductCommentId>, IAuditableBase
{
    public ProductId ProductId { get; private set; }
    public string UserName { get; private set; } = null!;
    public Guid UserId { get; private set; }
    public string Content { get; private set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime LastModifiedAt { get; set; }

    private ProductComment()
    {
    }

    private ProductComment(
        ProductCommentId id,
        ProductId productId,
        string userName,
        string content,
        Guid userId
    ) : base(id)
    {
        ProductId = productId;
        UserName = userName;
        Content = content;
        UserId = userId;
    }

    public static ProductComment Create(
        string userName,
        ProductId productId,
        string content,
        Guid userId
    )
    {
        return new(
            ProductCommentId.CreateUnique(),
            productId,
            userName,
            content,
            userId);
    }
}