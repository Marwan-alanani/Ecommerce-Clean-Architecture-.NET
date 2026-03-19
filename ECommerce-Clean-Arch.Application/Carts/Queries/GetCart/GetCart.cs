using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence.Repositories;
using ECommerce_Clean_Arch.Application.Carts.Models;

using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Carts.Queries.GetCart;

public sealed record GetCartQuery : IQuery<Cart>;

public sealed class GetCartQueryHandler : IQueryHandler<GetCartQuery, Cart>
{
    private readonly ICartRepository _cartRepository;

    public GetCartQueryHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<Result<Cart>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetCartAsync() ?? Cart.Create();
        return cart;
    }
}