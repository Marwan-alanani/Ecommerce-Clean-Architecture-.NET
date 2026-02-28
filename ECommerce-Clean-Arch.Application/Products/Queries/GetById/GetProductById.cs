using AutoMapper;
using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Persistence.Repositories;
using ECommerce_Clean_Arch.Domain.Errors.Products;
using ECommerce_Clean_Arch.Domain.Products.ValueObjects;
using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Products.Queries.GetById;

public record GetProductById(Guid Id) : IQuery<ProductDto>;

public class GetProductByIdQueryHandler : IQueryHandler<GetProductById, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Result<ProductDto>> Handle(
        GetProductById request,
        CancellationToken cancellationToken
    )
    {
        var product = await _productRepository.GetByIdAsync(
            ProductId.Create(request.Id),
            cancellationToken);
        if (product is null)
        {
            return Error.NotFound(new ProductNotFound(request.Id));
        }

        var productDto = _mapper.Map<ProductDto>(product);

        return productDto;
    }
}