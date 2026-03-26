namespace ECommerce_Clean_Arch.Application.Categories.Queries.GetPage;

public sealed record GetCategoryPageQuery(string? Name) : DefaultPageQuery<CategoryDto>;

public sealed class GetCategoryPageQueryHandler :
    IQueryHandler<GetCategoryPageQuery, PaginatedList<CategoryDto>>
{
    private readonly IApplicationDbContext _context;

    public GetCategoryPageQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PaginatedList<CategoryDto>>> Handle(
        GetCategoryPageQuery request,
        CancellationToken cancellationToken
    )
    {
        var categories = _context.Categories.AsNoTracking().Where(c => c.IsActive);
        if (request.Name is not null)
        {
            var s = request.Name.Trim();
            categories = categories.Where(c => EF.Functions.Like(c.Name, $"%{s}%"));
        }

        SortDirection direction = string.IsNullOrEmpty(request.Direction)
            ? SortDirection.Desc
            : Enum.Parse<SortDirection>(request.Direction, true);
        CategorySortingOptions sortBy = string.IsNullOrEmpty(request.SortBy)
            ? CategorySortingOptions.CreatedAt
            : Enum.Parse<CategorySortingOptions>(request.SortBy, true);

        categories = (direction, sortBy) switch
        {
            (SortDirection.Desc, CategorySortingOptions.CreatedAt) => categories
                .OrderByDescending(c => c.CreatedAt),
            (SortDirection.Asc, CategorySortingOptions.CreatedAt) => categories
                .OrderBy(c => c.CreatedAt),
            (SortDirection.Desc, CategorySortingOptions.Name) => categories
                .OrderByDescending(c => c.Name),
            (SortDirection.Asc, CategorySortingOptions.Name) => categories
                .OrderBy(c => c.Name),
            _ => throw new ArgumentOutOfRangeException()
        };
        return await categories
            .Select(c => new CategoryDto(c.Id.Value, c.Name))
            .PaginatedListAsync(
                request.PageNo,
                request.PageSize,
                cancellationToken);
    }
}