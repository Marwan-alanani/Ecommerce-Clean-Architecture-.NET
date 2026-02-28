using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Persistence;
using ECommerce_Clean_Arch.Application.Persistence.Repositories;
using ECommerce_Clean_Arch.Domain.Products;
using SharedKernel.Results;
using ECommerce_Clean_Arch.Application.Common.Models;
using ECommerce_Clean_Arch.Application.Products.Queries.GetById;

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
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProducts(
        IProductRepository productRepository,
        IMapper mapper,
        IApplicationDbContext context
    )
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<Result<PaginatedList<ProductDto>>> Handle(
        GetAllProductsQuery request,
        CancellationToken cancellationToken
    )
    {
        IQueryable<Product> products = _context.Products.Where(p => p.IsActive);
        if (request.Search is not null)
        {
            products = products.Where(p => p.Name.ToLower().Contains(request.Search.ToLower()));
        }

        Expression<Func<Product, object>> sortBy =
            Enum.Parse<ProductSortingOptions>(request.SortBy!, true) switch
            {
                ProductSortingOptions.Price => product => product.Price.Amount,
                ProductSortingOptions.CreatedAt => product => product.CreatedAt,
                ProductSortingOptions.Name => product => product.Name,
                _ => product => product.CreatedAt,
            };

        products = Enum.Parse<SortDirection>(request.Direction!, true) == SortDirection.Asc
            ? products.OrderBy(sortBy)
            : products.OrderByDescending(sortBy);

        var productDtoPage = await products
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(
                request.PageNo,
                request.PageSize,
                cancellationToken);
        return productDtoPage;
    }
}