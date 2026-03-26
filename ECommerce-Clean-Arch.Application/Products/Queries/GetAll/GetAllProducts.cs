namespace ECommerce_Clean_Arch.Application.Products.Queries.GetAll;

public sealed record GetAllProductsQuery : IQuery<PaginatedList<ProductDto>>
{
    public int PageNo { get; init; } = 1;
    public int PageSize { get; init; } = 5;
    public string? Search { get; init; }
    public string? SortBy { get; init; } = "createdAt";
    public string? Direction { get; init; } = "desc";
    public Guid? CategoryId { get; init; }
}

public class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, PaginatedList<ProductDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllProductsQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result<PaginatedList<ProductDto>>> Handle(
        GetAllProductsQuery request,
        CancellationToken cancellationToken
    )
    {
        var products = _context.Products.AsNoTracking().Where(p => p.IsActive);
        if (request.Search is not null)
        {
            var s = request.Search.Trim();
            products = products.Where(p => EF.Functions.Like(p.Name, $"%{s}%"));
        }

        if (request.CategoryId is not null)
        {
            products = products
                .Where(p => p.CategoryId == CategoryId.FromValue(request.CategoryId.Value));
        }

        ProductSortingOptions? sortBy = null;
        SortDirection? direction = null;

        if (request.SortBy != null)
            sortBy = Enum.Parse<ProductSortingOptions>(request.SortBy.Trim(), true);

        if (request.Direction != null)
            direction = Enum.Parse<SortDirection>(request.Direction.Trim(), true);

        products = (sortBy, direction) switch
        {
            (ProductSortingOptions.Name, SortDirection.Asc) => products.OrderBy(p => p.Name),

            (ProductSortingOptions.Name, SortDirection.Desc) => products.OrderByDescending(p => p.Name),
            (_, SortDirection.Asc) => products.OrderBy(p => p.CreatedAt),
            _ => products.OrderByDescending(p => p.CreatedAt)
        };

        var productDtoPage = await products
            .ToProductDto(_context)
            .PaginatedListAsync(
                request.PageNo,
                request.PageSize,
                cancellationToken);
        return productDtoPage;
    }
}