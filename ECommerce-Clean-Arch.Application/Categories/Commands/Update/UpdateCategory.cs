using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Persistence;
using ECommerce_Clean_Arch.Application.Persistence.Repositories;
using ECommerce_Clean_Arch.Domain.Categories.ValueObjects;
using ECommerce_Clean_Arch.Domain.Errors.Categories;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Categories.Commands.Update;

public sealed record UpdateCategoryCommand(
    CategoryId Id,
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
        var category = await _categoryRepository.GetCategoryAsync(request.Id, cancellationToken);
        if (category is null)
        {
            return Error.NotFound(new CategoryNotFound(request.Id));
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