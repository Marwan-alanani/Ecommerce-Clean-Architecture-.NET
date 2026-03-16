using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Application.Categories.Queries.Common;
using ECommerce_Clean_Arch.Domain.Errors.Categories;

using Microsoft.EntityFrameworkCore;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Categories.Queries.GetByName;

public sealed record GetCategoryByNameQuery(string Name) : IQuery<CategoryDto>;

public sealed class GetCategoryByNameQueryHandler
    : IQueryHandler<GetCategoryByNameQuery, CategoryDto>
{
    private readonly IApplicationDbContext _context;

    public GetCategoryByNameQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<CategoryDto>> Handle(
        GetCategoryByNameQuery request,
        CancellationToken cancellationToken
    )
    {
        var category = await _context.Categories.AsNoTracking()
            .Where(c => c.Name == request.Name)
            .Where(c => c.IsActive)
            .FirstOrDefaultAsync(cancellationToken);
        if (category is null)
        {
            return Error.NotFound(new CategoryNotFound(request.Name));
        }

        return new CategoryDto(
            category.Id.Value,
            category.Name
        );
    }
}