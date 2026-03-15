using AutoMapper;

using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Common.Models;
using ECommerce_Clean_Arch.Application.Persistence;
using ECommerce_Clean_Arch.Application.Persistence.Repositories;
using ECommerce_Clean_Arch.Domain.Categories.ValueObjects;
using ECommerce_Clean_Arch.Domain.Errors.Categories;
using ECommerce_Clean_Arch.Domain.Errors.Products;
using ECommerce_Clean_Arch.Domain.Products;
using ECommerce_Clean_Arch.Domain.Products.ValueObjects;

using SharedKernel.Errors;
using SharedKernel.Models;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Products.Commands.Create;

public record CreateProductCommand(
    string Name,
    string Description,
    MoneyDto Price,
    string PictureUrl,
    Guid? CategoryId
) : ICommand<ProductId>;

public class CreateProduct : ICommandHandler<CreateProductCommand, ProductId>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;

    public CreateProduct(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICategoryRepository categoryRepository
    )
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<ProductId>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken
    )
    {
        if (await _productRepository.NameExists(request.Name, cancellationToken))
        {
            return Error.Conflict(new ProductNameExists(request.Name));
        }

        CategoryId? categoryId = (request.CategoryId is not null)
            ? CategoryId.FromValue(request.CategoryId.Value)
            : null;

        if (categoryId is not null
            &&
            !(await _categoryRepository.CategoryExists(categoryId.Value, cancellationToken)))
        {
            return Error.NotFound(new CategoryNotFound(categoryId.Value));
        }

        var price = _mapper.Map<Money>(request.Price);
        var product = Product.Create(
            request.Name,
            request.Description,
            price,
            request.PictureUrl
        );
        product.SetCategoryId(categoryId);


        await _productRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}