namespace ECommerce_Clean_Arch.Contracts.Products;

public record CreateProductRequest(
    string Name,
    string Description,
    decimal Amount,
    string Currency,
    string PictureUrl
);