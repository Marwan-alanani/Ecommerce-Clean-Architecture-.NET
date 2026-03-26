using ECommerce_Clean_Arch.Domain.Common.Interfaces;

namespace ECommerce_Clean_Arch.Domain.Products.ValueObjects;

public readonly record struct ProductId : IAggregateRootId<ProductId>
{
    public Guid Value { get; }

    private ProductId(Guid value)
    {
        Value = value;
    }

    public ProductId()
    {
    }


    public static ProductId CreateUnique() => new ProductId(Guid.NewGuid());
    public static ProductId FromValue(Guid value) => new ProductId(value);
}