namespace ECommerce_Clean_Arch.Application.Carts.Commands.RemoveItem;

public sealed record RemoveItemInCartCommand(Guid ProductId) : ICommand;

public sealed class RemoveItemInCartCommandCommandHandler : ICommandHandler<RemoveItemInCartCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartKeyResolver _cartKeyResolver;


    public RemoveItemInCartCommandCommandHandler(
        ICartRepository cartRepository,
        ICartKeyResolver cartKeyResolver
    )
    {
        _cartRepository = cartRepository;
        _cartKeyResolver = cartKeyResolver;
    }

    public async Task<Result> Handle(
        RemoveItemInCartCommand request,
        CancellationToken cancellationToken
    )
    {
        var cartKey = _cartKeyResolver.GetCartKey();
        var cart = await _cartRepository.GetCartAsync(cartKey);
        if (cart is null)
        {
            return Result.Success();
        }

        cart.RemoveItem(request.ProductId);
        await _cartRepository.SetCartAsync(cartKey, cart);
        return Result.Success();
    }
}