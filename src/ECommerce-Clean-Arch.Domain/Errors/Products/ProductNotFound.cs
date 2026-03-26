using ECommerce_Clean_Arch.Domain.Products;
using ECommerce_Clean_Arch.Domain.Products.ValueObjects;

using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Products;

public record ProductNotFound : ErrorReason
{
    public ProductNotFound(ProductId id) : base(
        ErrorCodes.ProductNotFound,
        $"No product with id: {id.Value} found!",
        nameof(Product.Id))

    {
    }
}