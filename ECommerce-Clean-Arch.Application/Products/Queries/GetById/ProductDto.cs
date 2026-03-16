namespace ECommerce_Clean_Arch.Application.Products.Queries.GetById;

public record ProductDto
{
    public ProductDto(
        Guid id,
        string name,
        string? description,
        string currency,
        decimal price,
        string? categoryName,
        DateTime createdAt,
        DateTime lastModifiedAt
    )
    {
        Id = id;
        Name = name;
        Description = description;
        Currency = currency;
        Price = price;
        CategoryName = categoryName;
        CreatedAt = createdAt;
        LastModifiedAt = lastModifiedAt;
    }

    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public string Currency { get; init; } = null!;
    public string? CategoryName { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime LastModifiedAt { get; init; }
}