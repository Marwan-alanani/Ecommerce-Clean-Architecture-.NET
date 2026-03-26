namespace ECommerce_Clean_Arch.Application.Categories.Commands.Create;

public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(c => c.Name)
            .Length(3, 50)
            .WithMessage("Category name must be between 3 and 50 characters long");
    }
}