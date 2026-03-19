using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Application.Common.Interfaces;
using ECommerce_Clean_Arch.Domain.Carts;
using ECommerce_Clean_Arch.Domain.Carts.ValueObjects;

using Microsoft.EntityFrameworkCore;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Carts.Commands.Create;

public sealed record CreateCartCommand : ICommand<CartId>;

public sealed class CreateCartCommandHandler
    : ICommandHandler<CreateCartCommand, CartId>
{
    private readonly IUser _user;
    private readonly IApplicationDbContext _context;

    public CreateCartCommandHandler(IUser user, IApplicationDbContext context)
    {
        _user = user;
        _context = context;
    }

    public async Task<Result<CartId>> Handle(
        CreateCartCommand request,
        CancellationToken cancellationToken
    )
    {
        if (_user.Id is null)
        {
            return Error.Security();
        }

        var oldCartExists = await _context.Carts
            .Where(c => c.UserId == _user.Id)
            .AnyAsync(cancellationToken);
        if (oldCartExists)
        {
            return Error.Conflict();
        }

        var cart = Cart.Create(_user.Id.Value);
        await _context.Carts.AddAsync(cart, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return cart.Id;
    }
}