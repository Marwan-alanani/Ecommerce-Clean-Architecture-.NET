using ECommerce_Clean_Arch.Domain.Products;

using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Products;

public record ProductNotFound(
    Guid Id
) : ErrorReason(
    ErrorCodes.ProductNotFound,
    $"No product with id: {Id} found!",
    nameof(Product.Id))
{
}