using ECommerce_Clean_Arch.Domain.Common.Interfaces;

namespace ECommerce_Clean_Arch.Domain.ProductComments.ValueObjects;

public readonly record struct ProductCommentId : IAggregateRootId<ProductCommentId>
{
    public Guid Value { get; }

    private ProductCommentId(Guid value)
    {
        Value = value;
    }

    public static ProductCommentId CreateUnique() => new(Guid.NewGuid());
    public static ProductCommentId FromValue(Guid value) => new(value);
}