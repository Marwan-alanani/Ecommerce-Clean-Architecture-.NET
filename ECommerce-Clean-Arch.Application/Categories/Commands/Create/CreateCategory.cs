using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Domain.Categories;
using ECommerce_Clean_Arch.Domain.Categories.ValueObjects;
using ECommerce_Clean_Arch.Domain.Errors.Categories;

using Microsoft.EntityFrameworkCore;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Categories.Commands.Create;

public sealed record CreateCategoryCommand(string Name) : ICommand<CategoryId>;

public sealed class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, CategoryId>
{
    private readonly IApplicationDbContext _context;

    public CreateCategoryCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result<CategoryId>> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken
    )
    {
        var categoryNameExists = await _context.Categories
            .AnyAsync(c => c.Name == request.Name, cancellationToken);
        if (categoryNameExists)
        {
            return Error.Conflict(new CategoryNameAlreadyExists(request.Name));
        }

        var category = Category.Create(request.Name);
        await _context.Categories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return category.Id;
    }
}