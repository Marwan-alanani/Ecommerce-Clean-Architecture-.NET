using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence.Repositories;
using ECommerce_Clean_Arch.Application.Carts.Models;
using ECommerce_Clean_Arch.Application.Products.Common;
using ECommerce_Clean_Arch.Domain.Errors.Products;
using ECommerce_Clean_Arch.Domain.Products.ValueObjects;

using Microsoft.EntityFrameworkCore;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Carts.Commands.SetItem;

public sealed record SetItemInCartCommand(ProductId ProductId, int Quantity) : ICommand;

public sealed class SetItemInCartCommandHandler : ICommandHandler<SetItemInCartCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly IApplicationDbContext _context;

    public SetItemInCartCommandHandler(
        ICartRepository cartRepository,
        IApplicationDbContext context
    )
    {
        _cartRepository = cartRepository;
        _context = context;
    }

    public async Task<Result> Handle(SetItemInCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetCartAsync() ?? Cart.Create();

        var productData = await _context.Products.AsNoTracking()
            .Where(p => p.Id == request.ProductId && p.IsActive)
            .ToProductData()
            .FirstOrDefaultAsync(cancellationToken);
        if (productData is null)
        {
            return Error.NotFound(new ProductNotFound(request.ProductId));
        }

        var result = cart.SetCartItem(productData, request.Quantity);
        if (result.IsFailure) return result.Error;
        await _cartRepository.SetCartAsync(cart);
        return Result.Success();
    }
}