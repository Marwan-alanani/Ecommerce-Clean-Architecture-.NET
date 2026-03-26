namespace ECommerce_Clean_Arch.Application.Products.Queries.Common;

public record ProductDto
{
    public ProductDto(
        Guid id,
        string name,
        string? description,
        string? categoryName,
        DateTime createdAt,
        DateTime lastModifiedAt
    )
    {
        Id = id;
        Name = name;
        Description = description;
        CategoryName = categoryName;
        CreatedAt = createdAt;
        LastModifiedAt = lastModifiedAt;
    }

    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public string? CategoryName { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime LastModifiedAt { get; init; }
}