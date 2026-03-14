using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Persistence;
using ECommerce_Clean_Arch.Application.Persistence.Repositories;
using ECommerce_Clean_Arch.Domain.Errors.Products;
using ECommerce_Clean_Arch.Domain.Products.ValueObjects;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Products.Commands.Deactivate;

public record DeactivateProductCommand(Guid Id) : ICommand
{
}

public class DeactivateProductHandler : ICommandHandler<DeactivateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateProductHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeactivateProductCommand request,
        CancellationToken cancellationToken
    )
    {
        var product = await _productRepository.GetByIdAsync(
            ProductId.FromValue(request.Id),
            cancellationToken);
        if (product is null)
        {
            return Error.NotFound(new ProductNotFound(request.Id));
        }

        product.Deactivate();
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            return Error.InternalServerError(e);
        }

        return Result.Success();
    }
}