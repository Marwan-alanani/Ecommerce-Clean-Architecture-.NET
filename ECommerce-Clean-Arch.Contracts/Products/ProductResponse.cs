namespace ECommerce_Clean_Arch.Contracts.Products;

public record ProductResponse(
    string Name,
    string Description,
    decimal Amount,
    string Currency,
    DateTime CreatedAt,
    DateTime UpdatedAt
);