namespace ECommerce_Clean_Arch.Application.Carts.Queries.GetCart;

public sealed record GetCartQuery : IQuery<Cart>;

public sealed class GetCartQueryHandler : IQueryHandler<GetCartQuery, Cart>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartKeyResolver _keyResolver;

    public GetCartQueryHandler(ICartRepository cartRepository, ICartKeyResolver keyResolver)
    {
        _cartRepository = cartRepository;
        _keyResolver = keyResolver;
    }

    public async Task<Result<Cart>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetCartAsync(_keyResolver.GetCartKey()) ?? Cart.Create();
        return cart;
    }
}