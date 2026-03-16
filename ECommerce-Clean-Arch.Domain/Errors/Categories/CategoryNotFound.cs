using ECommerce_Clean_Arch.Domain.Categories;
using ECommerce_Clean_Arch.Domain.Categories.ValueObjects;

using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Categories;

public sealed record CategoryNotFound : ErrorReason
{
    public CategoryNotFound(CategoryId id) : base(
        nameof(CategoryNotFound),
        $"No category with the given {id} was found.",
        nameof(Category.Id))
    {
    }

    public CategoryNotFound(string name) : base(
        nameof(CategoryNotFound),
        $"No category with the given name {name} was found.",
        nameof(Category.Name))
    {
    }
}