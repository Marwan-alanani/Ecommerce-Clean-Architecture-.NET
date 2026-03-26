using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Products;

public record NoProductsFound() : ErrorReason(ErrorCodes.NoProductsFound, "No products in database");