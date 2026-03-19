using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Application.Products.Queries.Common;
using ECommerce_Clean_Arch.Domain.Products;

namespace ECommerce_Clean_Arch.Application.Products.Common;

public static class ProductQueryExtensions
{
    public static IQueryable<ProductDto> ToProductDto(
        this IQueryable<Product> products,
        IApplicationDbContext context
    )
    {
        return products.Select(p => new ProductDto(
                p.Id.Value,
                p.Name,
                p.Description,
                context.Categories
                    .Where(c => c.Id == p.CategoryId)
                    .Where(c => c.IsActive)
                    .Select(c => c.Name)
                    .FirstOrDefault(),
                p.CreatedAt,
                p.LastModifiedAt
            )
        );
    }
}