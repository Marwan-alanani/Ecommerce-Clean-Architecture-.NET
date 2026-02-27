using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Persistence;
using ECommerce_Clean_Arch.Application.Persistence.Repositories;
using ECommerce_Clean_Arch.Domain.Errors.Products;
using ECommerce_Clean_Arch.Domain.Products;
using SharedKernel.Errors;
using SharedKernel.Models;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken
    )
    {
        if (await _productRepository.NameExists(request.Name, cancellationToken))
        {
            return Error.Conflict(new ProductNameExists(request.Name));
        }

        var price = new Money(Enum.Parse<Currency>(request.Currency), request.Price);
        var product = Product.Create(
            request.Name,
            request.Description,
            price,
            request.PictureUrl);


        await _productRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return product.Id.Value;
    }
}