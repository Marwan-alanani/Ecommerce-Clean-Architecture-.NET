using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence.Repositories;

using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Carts.Commands.Remove;

public sealed record RemoveCartCommand : ICommand;

public sealed class RemoveCartCommandHandler : ICommandHandler<RemoveCartCommand>
{
    private readonly ICartRepository _cartRepository;

    public RemoveCartCommandHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<Result> Handle(RemoveCartCommand request, CancellationToken cancellationToken)
    {
        await _cartRepository.RemoveCartAsync();
        return Result.Success();
    }
}