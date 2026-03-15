using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Persistence;
using ECommerce_Clean_Arch.Application.Persistence.Repositories;
using ECommerce_Clean_Arch.Domain.Categories.ValueObjects;
using ECommerce_Clean_Arch.Domain.Errors.Categories;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Categories.Update;

public sealed record UpdateCategoryCommand(
    Guid Id,
    string Name
) : ICommand;

public sealed class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryId = CategoryId.FromValue(request.Id);
        var category = await _categoryRepository.GetCategoryAsync(categoryId, cancellationToken);
        if (category is null)
        {
            return Error.NotFound(new CategoryNotFound(categoryId));
        }

        if (category.Name == request.Name)
        {
            return Result.Success();
        }

        var nameExists = await _categoryRepository.CategoryExists(request.Name, cancellationToken);
        if (nameExists)
        {
            return Error.Conflict(new CategoryNameAlreadyExists(request.Name));
        }

        category.Name = request.Name;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}