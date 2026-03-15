using ECommerce_Clean_Arch.Domain.Categories;

using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Categories;

public sealed record CategoryNameAlreadyExists : ErrorReason
{
    public CategoryNameAlreadyExists(string name)
        : base(
            nameof(CategoryNameAlreadyExists),
            $"Category with name {name} already exists.",
            nameof(Category.Name))
    {
    }
}