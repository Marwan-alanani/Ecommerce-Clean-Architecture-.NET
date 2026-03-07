using FluentValidation;

namespace ECommerce_Clean_Arch.Application.Authentication.Queries.Login;

public class LoginQueryValidator : AbstractValidator<Login>
{
    public LoginQueryValidator()
    {
        RuleFor(loginQuery => loginQuery.Email).EmailAddress().NotEmpty();

        RuleFor(loginQuery => loginQuery.Password).NotEmpty().MinimumLength(6);

    }
}