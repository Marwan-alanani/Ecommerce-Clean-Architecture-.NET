using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Application.Common.Interfaces;
using ECommerce_Clean_Arch.Application.Common.Models;
using ECommerce_Clean_Arch.Application.Orders.Queries.GetById;
using ECommerce_Clean_Arch.Domain.Errors.Security;

using Microsoft.EntityFrameworkCore;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Orders.Queries.GetPage;

public sealed record GetOrdersPageQuery : DefaultPageQuery<OrderDto>;

public sealed class GetOrdersPageQueryHandler :
    IQueryHandler<GetOrdersPageQuery, PaginatedList<OrderDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public GetOrdersPageQueryHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task<Result<PaginatedList<OrderDto>>> Handle(
        GetOrdersPageQuery request,
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