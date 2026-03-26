using ECommerce_Clean_Arch.Domain.Products;

using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Products;

public record ProductNameExists(string Name) : ErrorReason(
    ErrorCodes.ProductNameExists,
    $"Product with the name: {Name} exists!",
    nameof(Product.Name))
{
}