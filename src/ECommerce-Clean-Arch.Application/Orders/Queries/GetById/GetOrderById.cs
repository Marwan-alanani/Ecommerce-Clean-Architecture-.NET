namespace ECommerce_Clean_Arch.Application.Orders.Queries.GetById;

public sealed record GetOrderByIdQuery(OrderId OrderId) : IQuery<OrderDto>;

public sealed class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, OrderDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public GetOrderByIdQueryHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task<Result<OrderDto>> Handle(
        GetOrderByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        if (_user.Id is null)
        {
            return Error.Security(new UserUnauthenticated());
        }

        var order = await _context.Orders.AsNoTracking()
            .Where(o => o.Id == request.OrderId)
            .FirstOrDefaultAsync(cancellationToken);

        if (order is null)
        {
            return Error.NotFound(new OrderNotFound(request.OrderId));
        }

        if ((!_user.Permissions?.Contains(Permissions.Orders.ViewAll) ?? true)
            || order.UserId != _user.Id.Value)
        {
            return Error.Security(new UserUnauthorized());
        }

        return order.ToDto();
    }
}