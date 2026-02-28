using FluentValidation;
using SharedKernel.Models;

namespace ECommerce_Clean_Arch.Application.Products.Commands.Update;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(command => command.Price!.Amount)
            .GreaterThan(0)
            .When(command => command.Price is not null)
            .WithMessage($"Price must be greater than zero");

        RuleFor(command => command.Price!.Currency)
            .IsEnumName(typeof(Currency),false)
            .When(command => command.Price is not null);
    }
}