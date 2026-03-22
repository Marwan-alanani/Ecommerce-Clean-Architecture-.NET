using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Application.Products.Queries.Common;
using ECommerce_Clean_Arch.Domain.Carts;
using ECommerce_Clean_Arch.Domain.Products;

using SharedKernel.Models;

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

    public static IQueryable<ProductData> ToProductData(
        this IQueryable<Product> products
    )
    {
        return products.Select(p => new ProductData
        {
            Id = p.Id.Value,
            Name = p.Name,
            PictureUrl = p.PictureUrl,
            Price = new MoneyFlat(p.Price.Currency.ToString(), p.Price.Amount)
        });
    }
}