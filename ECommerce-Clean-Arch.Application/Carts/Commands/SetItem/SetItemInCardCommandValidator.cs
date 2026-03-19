using FluentValidation;

namespace ECommerce_Clean_Arch.Application.Carts.Commands.SetItem;

public sealed class SetItemInCardCommandValidator : AbstractValidator<SetItemInCartCommand>
{
    public SetItemInCardCommandValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");
    }
}