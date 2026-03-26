namespace ECommerce_Clean_Arch.Application.Products.Commands.Create;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Price.Amount)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0")
            .NotEmpty();

        RuleFor(x => x.Price.Currency)
            .IsEnumName(typeof(Currency), false)
            .NotEmpty();

        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Description);
        RuleFor(x => x.PictureUrl)
            .NotEmpty()
            .WithMessage("PictureUrl is required");
        // TODO: validator that the picture url is an actual picture url
    }
}