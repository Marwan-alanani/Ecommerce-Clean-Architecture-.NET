using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Persistence;
using ECommerce_Clean_Arch.Domain.Products;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Product>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Product>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken
    )
    {
        var product = Product.Create(
            request.Name,
            request.Description,
            request.Price,
            request.PictureUrl);
        await _productRepository.AddAsync(product,cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return product;
    }
}