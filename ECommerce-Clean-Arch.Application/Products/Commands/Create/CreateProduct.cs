using AutoMapper;

using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Common.Models;
using ECommerce_Clean_Arch.Application.Persistence;
using ECommerce_Clean_Arch.Application.Persistence.Repositories;
using ECommerce_Clean_Arch.Domain.Errors.Products;
using ECommerce_Clean_Arch.Domain.Products;

using SharedKernel.Errors;
using SharedKernel.Models;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Products.Commands.Create;

public record CreateProductCommand(
    string Name,
    string Description,
    MoneyDto Price,
    string PictureUrl
) : ICommand<EntityCreatedDto>;

public class CreateProduct : ICommandHandler<CreateProductCommand, EntityCreatedDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProduct(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<EntityCreatedDto>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken
    )
    {
        if (await _productRepository.NameExists(request.Name, cancellationToken))
        {
            return Error.Conflict(new ProductNameExists(request.Name));
        }

        var price = _mapper.Map<Money>(request.Price);
        var product = Product.Create(
            request.Name,
            request.Description,
            price,
            request.PictureUrl
        );


        await _productRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new EntityCreatedDto(product.Id.Value);
    }
}