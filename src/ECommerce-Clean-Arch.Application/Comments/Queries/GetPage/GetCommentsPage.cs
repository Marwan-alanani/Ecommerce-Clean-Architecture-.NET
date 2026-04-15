namespace ECommerce_Clean_Arch.Application.Comments.Queries.GetPage;

public sealed record GetCommentsPageQuery : IQuery<PaginatedList<ProductCommentDto>>
{
    public const int PageSize = 10;
    public const int DefaultPageNo = 1;
    public required int? PageNo { get; init; } = 1;
    public required ProductId ProductId { get; init; }
}

public sealed class GetCommentsPageQueryHandler : IQueryHandler<GetCommentsPageQuery,
    PaginatedList<ProductCommentDto>>
{
    private readonly IApplicationDbContext _context;

    public GetCommentsPageQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PaginatedList<ProductCommentDto>>> Handle(
        GetCommentsPageQuery request,
        CancellationToken cancellationToken
    )
    {
        var comments = _context.ProductComments
            .AsNoTracking()
            .Where(c => c.ProductId == request.ProductId)
            .OrderByDescending(c => c.CreatedAt);


        return await comments
            .ToDto()
            .PaginatedListAsync(
                request.PageNo ?? GetCommentsPageQuery.DefaultPageNo,
                GetCommentsPageQuery.PageSize,
                cancellationToken);
    }
}