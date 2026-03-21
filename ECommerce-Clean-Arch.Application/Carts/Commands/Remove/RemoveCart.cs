using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence.Repositories;
using ECommerce_Clean_Arch.Application.Services;

using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Carts.Commands.Remove;

public sealed record RemoveCartCommand : ICommand;

public sealed class RemoveCartCommandHandler : ICommandHandler<RemoveCartCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartKeyResolver _keyResolver;

    public RemoveCartCommandHandler(ICartRepository cartRepository, ICartKeyResolver keyResolver)
    {
        _cartRepository = cartRepository;
        _keyResolver = keyResolver;
    }

    public async Task<Result> Handle(RemoveCartCommand request, CancellationToken cancellationToken)
    {
        await _cartRepository.RemoveCartAsync(_keyResolver.GetCartKey());
        return Result.Success();
    }
}