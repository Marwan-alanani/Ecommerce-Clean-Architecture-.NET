using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Persistence;
using ECommerce_Clean_Arch.Application.Persistence.Repositories;
using ECommerce_Clean_Arch.Application.Products.Queries.GetProductById;
using ECommerce_Clean_Arch.Domain.Products;
using SharedKernel.Results;
using ECommerce_Clean_Arch.Application.Common.Models;

namespace ECommerce_Clean_Arch.Application.Products.Queries.GetAll;

public class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, PaginatedList<ProductDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductsQueryHandler(
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
        IQueryable<Product> products = _context.Products;
        if (request.Search is not null)
        {
            products = products.Where(p => p.Name.ToLower().Contains(request.Search.ToLower()));
        }

        Expression<Func<Product, object>> sortBy =
            Enum.Parse<ProductSoringOptions>(request.SortBy!, true) switch
            {
                ProductSoringOptions.Price => product => product.Price,
                ProductSoringOptions.CreatedAt => product => product.CreatedAt,
                ProductSoringOptions.Name => product => product.Name,
                _ => product => product.CreatedAt,
            };

        var dir = Enum.Parse<SortDirection>(request.Direction!, true);
        products = dir == SortDirection.Asc
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