using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Application.Common.Interfaces;

using Microsoft.EntityFrameworkCore;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Carts.Queries.GetUserCart;

public sealed record GetUserCartQuery : IQuery<CartDto>;

public sealed class GetUserCartQueryHandler :
    IQueryHandler<GetUserCartQuery, CartDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public GetUserCartQueryHandler(
        IApplicationDbContext context,
        IUser user
    )
    {
        _context = context;
        _user = user;
    }

    public async Task<Result<CartDto>> Handle(
        GetUserCartQuery request,
        CancellationToken cancellationToken
    )
    {
        if (_user.Id == null)
        {
            return Error.Security();
        }

        var cart = await _context.Carts.AsNoTracking()
            .Where(c => c.UserId == _user.Id.Value)
            .FirstOrDefaultAsync(cancellationToken);

        return (cart == null) ? Error.NotFound() : cart.ToDto();
    }
}