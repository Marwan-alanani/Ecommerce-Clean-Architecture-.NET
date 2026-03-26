namespace ECommerce_Clean_Arch.Application.Categories.Commands.Update;

public sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(c => c.Name)
            .Length(3, 50)
            .WithMessage("Category name must be between 3 and 50 characters long");
    }
}