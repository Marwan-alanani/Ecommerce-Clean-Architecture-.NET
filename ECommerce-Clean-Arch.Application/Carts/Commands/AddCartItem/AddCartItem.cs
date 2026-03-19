using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Application.Common.Interfaces;
using ECommerce_Clean_Arch.Domain.Carts.ValueObjects;
using ECommerce_Clean_Arch.Domain.Products.ValueObjects;

using Microsoft.EntityFrameworkCore;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Carts.Commands.AddCartItem;

public sealed record AddCartItemCommand(
    ProductId ProductId,
    int Quantity
) : ICommand<CartItemId>;

public sealed class AddCartItemCommandHandler : ICommandHandler<AddCartItemCommand, CartItemId>
{
    private readonly IUser _user;
    private readonly IApplicationDbContext _context;

    public AddCartItemCommandHandler(IUser user, IApplicationDbContext context)
    {
        _user = user;
        _context = context;
    }

    public async Task<Result<CartItemId>> Handle(
        AddCartItemCommand request,
        CancellationToken cancellationToken
    )
    {
        if (!_user.Id.HasValue)
        {
            return Error.Security();
        }

        var cart = await _context.Carts
            .Where(c => c.UserId == _user.Id.Value)
            .FirstOrDefaultAsync(cancellationToken);
        if (cart is null)
        {
            // or just create a cart yourself ??
            return Error.NotFound();
        }

        var product = await _context.Products
            .Where(p => p.Id == request.ProductId)
            .FirstOrDefaultAsync(cancellationToken);

        if (product is null) return Error.NotFound();


        var result = cart.AddCartItem(product, request.Quantity);
        if (result.IsFailure)
            return result;


        await _context.SaveChangesAsync(cancellationToken);
        return result;
    }
}