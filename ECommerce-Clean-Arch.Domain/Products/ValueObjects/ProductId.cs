namespace ECommerce_Clean_Arch.Domain.Products.ValueObjects;

public record struct ProductId
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
    public static ProductId Create(Guid value) => new ProductId(value);
}