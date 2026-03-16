using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Domain.Errors.Products;
using ECommerce_Clean_Arch.Domain.Products.ValueObjects;

using Microsoft.EntityFrameworkCore;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Products.Commands.Deactivate;

public record DeactivateProductCommand(ProductId Id) : ICommand;

public class DeactivateProductHandler : ICommandHandler<DeactivateProductCommand>
{
    private readonly IApplicationDbContext _context;

    public DeactivateProductHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result> Handle(
        DeactivateProductCommand request,
        CancellationToken cancellationToken
    )
    {
        var product = await _context.Products
            .Where(p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (product is null)
        {
            return Error.NotFound(new ProductNotFound(request.Id));
        }

        product.Deactivate();
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            return Error.InternalServerError(e);
        }

        return Result.Success();
    }
}