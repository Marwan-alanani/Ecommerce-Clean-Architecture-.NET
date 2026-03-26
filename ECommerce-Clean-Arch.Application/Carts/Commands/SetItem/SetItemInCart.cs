
namespace ECommerce_Clean_Arch.Application.Carts.Commands.SetItem;

public sealed record SetItemInCartCommand(ProductId ProductId, int Quantity) : ICommand;

public sealed class SetItemInCartCommandHandler : ICommandHandler<SetItemInCartCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly IApplicationDbContext _context;
    private readonly ICartKeyResolver _keyResolver;

    public SetItemInCartCommandHandler(
        ICartRepository cartRepository,
        IApplicationDbContext context,
        ICartKeyResolver keyResolver
    )
    {
        _cartRepository = cartRepository;
        _context = context;
        _keyResolver = keyResolver;
    }

    public async Task<Result> Handle(SetItemInCartCommand request, CancellationToken cancellationToken)
    {
        var cartKey = _keyResolver.GetCartKey();
        var cart = await _cartRepository.GetCartAsync(cartKey) ?? Cart.Create();

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
        await _cartRepository.SetCartAsync(cartKey, cart);
        return Result.Success();
    }
}