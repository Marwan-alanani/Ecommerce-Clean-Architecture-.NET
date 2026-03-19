using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence.Repositories;

using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Carts.Commands.RemoveItem;

public sealed record RemoveItemInCartCommand(Guid ProductId) : ICommand;

public sealed class RemoveItemInCartCommandCommandHandler : ICommandHandler<RemoveItemInCartCommand>
{
    private readonly ICartRepository _cartRepository;

    public RemoveItemInCartCommandCommandHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<Result> Handle(
        RemoveItemInCartCommand request,
        CancellationToken cancellationToken
    )
    {
        var cart = await _cartRepository.GetCartAsync();
        if (cart is null)
        {
            return Result.Success();
        }

        cart.RemoveItem(request.ProductId);
        await _cartRepository.SetCartAsync(cart);
        return Result.Success();
    }
}