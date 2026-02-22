using FluentValidation;

namespace ECommerce_Clean_Arch.Application.Authentication.Commands.RegisterUser;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(registerCommand => registerCommand.Username)
            .NotEmpty();

        RuleFor(registerCommand => registerCommand.Email)
            .EmailAddress();
        RuleFor(registerCommand => registerCommand.Password)
            .NotEmpty()
            .MinimumLength(6);

        RuleFor(registerCommand => registerCommand.FirstName)
            .MaximumLength(50)
            .NotEmpty();

        RuleFor(registerCommand => registerCommand.LastName)
            .MaximumLength(50)
            .NotEmpty();
    }
}