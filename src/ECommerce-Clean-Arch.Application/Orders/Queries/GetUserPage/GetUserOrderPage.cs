namespace ECommerce_Clean_Arch.Application.Orders.Queries.GetUserPage;

public sealed record GetUserOrdersPageQuery : DefaultPageQuery<OrderDto>;

public sealed class GetUserOrdersPageQueryHandler : IQueryHandler<GetUserOrdersPageQuery,
    PaginatedList<OrderDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public GetUserOrdersPageQueryHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }


    public async Task<Result<PaginatedList<OrderDto>>> Handle(
        GetUserOrdersPageQuery request,
        CancellationToken cancellationToken
    )
    {
        if (_user.Id is null)
        {
            return Error.Security(new UserUnauthenticated());
        }

        var userId = _user.Id.Value;
        var orders = _context.Orders.AsNoTracking().Where(o => o.UserId == userId);

        var dir = request.Direction is null
            ? SortDirection.Desc
            : Enum.Parse<SortDirection>(request.Direction, true);
        var sortBy = request.SortBy is null
            ? OrderSortingOptions.CreatedAt
            : Enum.Parse<OrderSortingOptions>(request.SortBy, true);


        orders = sortBy switch
        {
            OrderSortingOptions.CreatedAt => orders.OrderBy(o => o.CreatedAt),
            OrderSortingOptions.ItemCount => orders.OrderBy(o => o.Items.Count),
            OrderSortingOptions.Status => orders.OrderBy(o => o.Status),
            _ => throw new ArgumentOutOfRangeException()
        };
        if (dir == SortDirection.Desc)
        {
            orders = orders.Reverse();
        }

        return await orders
            .Select(o => o.ToDto())
            .PaginatedListAsync(
                request.PageNo,
                request.PageSize,
                cancellationToken);
    }
}