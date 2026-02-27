using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Common.Models;
using ECommerce_Clean_Arch.Application.Products.Queries.GetProductById;

namespace ECommerce_Clean_Arch.Application.Products.Queries.GetAll;

public record GetAllProductsQuery : IQuery<PaginatedList<ProductDto>>
{
    public int PageNo { get; init; } = 1;
    public int PageSize { get; init; } = 5;
    public string? Search { get; init; }
    public string? SortBy { get; init; } = "CreatedAt";
    public string? Direction { get; init; } = "desc";
}