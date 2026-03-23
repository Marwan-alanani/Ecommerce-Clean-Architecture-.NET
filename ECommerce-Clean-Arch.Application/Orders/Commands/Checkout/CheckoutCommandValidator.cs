using FluentValidation;

namespace ECommerce_Clean_Arch.Application.Orders.Commands.Checkout;

public sealed class CheckoutCommandValidator : AbstractValidator<CheckoutCommand>
{
    public CheckoutCommandValidator()
    {
        RuleFor(c => c.ShippingAddress.Street)
            .MaximumLength(200)
            .MinimumLength(3);
        RuleFor(c => c.ShippingAddress.City)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(c => c.ShippingAddress.Country)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);

        RuleFor(c => c.ShippingAddress.PostalCode)
            .NotEmpty()
            .MaximumLength(20)
            .MinimumLength(3);
    }
}