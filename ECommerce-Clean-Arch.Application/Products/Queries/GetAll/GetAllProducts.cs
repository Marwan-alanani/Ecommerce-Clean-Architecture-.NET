using AutoMapper;
using AutoMapper.QueryableExtensions;

using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Common.Models;
using ECommerce_Clean_Arch.Application.Persistence.Repositories;
using ECommerce_Clean_Arch.Application.Products.Queries.GetById;
using ECommerce_Clean_Arch.Domain.Products;

using Microsoft.EntityFrameworkCore;

using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Products.Queries.GetAll;

public record GetAllProductsQuery : IQuery<PaginatedList<ProductDto>>
{
    public int PageNo { get; init; } = 1;
    public int PageSize { get; init; } = 5;
    public string? Search { get; init; }
    public string? SortBy { get; init; } = "createdAt";
    public string? Direction { get; init; } = "desc";
}

public class GetAllProducts : IQueryHandler<GetAllProductsQuery, PaginatedList<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetAllProducts(
        IMapper mapper,
        IProductRepository productRepository
    )
    {
        _mapper = mapper;
        _productRepository = productRepository;
    }

    public async Task<Result<PaginatedList<ProductDto>>> Handle(
        GetAllProductsQuery request,
        CancellationToken cancellationToken
    )
    {
        IQueryable<Product> products = _productRepository.Products.Where(p => p.IsActive);
        if (request.Search is not null)
        {
            var s = request.Search.Trim();
            products = products.Where(p => EF.Functions.Like(p.Name, $"%{s}%"));
        }

        ProductSortingOptions? sortBy = null;
        SortDirection? direction = null;

        if (request.SortBy != null)
            sortBy = Enum.Parse<ProductSortingOptions>(request.SortBy.Trim(), true);

        if (request.Direction != null)
            direction = Enum.Parse<SortDirection>(request.Direction.Trim(), true);

        products = (sortBy, direction) switch
        {
            (ProductSortingOptions.Price, SortDirection.Asc) => products.OrderBy(p => p.Price.Amount),

            (ProductSortingOptions.Price, SortDirection.Desc) => products
                .OrderByDescending(p => p.Price.Amount),

            (ProductSortingOptions.Name, SortDirection.Asc) => products.OrderBy(p => p.Name),

            (ProductSortingOptions.Name, SortDirection.Desc) => products.OrderByDescending(p => p.Name),
            (_, SortDirection.Asc) => products.OrderBy(p => p.CreatedAt),
            _ => products.OrderByDescending(p => p.CreatedAt)
        };

        var productDtoPage = await products
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(
                request.PageNo,
                request.PageSize,
                cancellationToken);
        return productDtoPage;
    }
}