using AutoMapper;

using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Common.Models;
using ECommerce_Clean_Arch.Application.Persistence;
using ECommerce_Clean_Arch.Application.Persistence.Repositories;
using ECommerce_Clean_Arch.Domain.Errors.Products;
using ECommerce_Clean_Arch.Domain.Products;
using ECommerce_Clean_Arch.Domain.Products.ValueObjects;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Products.Commands.Update;

public record UpdateProductCommand(
    Guid Id,
    string? Name,
    string? Description,
    MoneyDto? Price
) : ICommand
{
    private class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<UpdateProductCommand, Product>()
                .ForAllMembers(opt => opt.Condition((
                    _,
                    _,
                    srcMember
                ) => srcMember is not null));
        }
    }
}

public class UpdateProduct : ICommandHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProduct(
        IProductRepository productRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork
    )
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(ProductId.Create(request.Id));
        if (product is null)
            return Error.NotFound(new ProductNotFound(request.Id));
        if (request.Name is not null && await _productRepository.NameExists(request.Name))
        {
            return Error.Validation(new ProductNameExists(request.Name));
        }

        _mapper.Map(request, product);
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