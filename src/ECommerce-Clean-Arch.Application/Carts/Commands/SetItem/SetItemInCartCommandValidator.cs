
namespace ECommerce_Clean_Arch.Application.Carts.Commands.SetItem;

public sealed class SetItemInCartCommandValidator : AbstractValidator<SetItemInCartCommand>
{
    public SetItemInCartCommandValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");
    }
}