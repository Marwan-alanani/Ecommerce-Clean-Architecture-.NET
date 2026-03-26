namespace ECommerce_Clean_Arch.Application.Categories.Queries.GetById;

public sealed record GetCategoryByIdQuery(CategoryId Id) : IQuery<CategoryDto>;

public sealed class GetCategoryByIdQueryHandler
    : IQueryHandler<GetCategoryByIdQuery, CategoryDto>
{
    private readonly IApplicationDbContext _context;

    public GetCategoryByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<CategoryDto>> Handle(
        GetCategoryByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var category = await _context.Categories.AsNoTracking()
            .Where(c => c.Id == request.Id)
            .Where(c => c.IsActive)
            .FirstOrDefaultAsync(cancellationToken);
        if (category is null)
        {
            return Error.NotFound(new CategoryNotFound(request.Id));
        }

        return new CategoryDto(
            category.Id.Value,
            category.Name
        );
    }
}