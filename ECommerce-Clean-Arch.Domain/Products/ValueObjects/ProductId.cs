namespace ECommerce_Clean_Arch.Domain.Products.ValueObjects;

public record struct ProductId(Guid Value)
{
    public static ProductId CreateUnique() => new ProductId(Guid.NewGuid());
    public static ProductId Create(Guid value) => new ProductId(value);
}