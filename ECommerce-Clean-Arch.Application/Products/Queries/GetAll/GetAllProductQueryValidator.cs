using ECommerce_Clean_Arch.Application.Common.Models;
using FluentValidation;

namespace ECommerce_Clean_Arch.Application.Products.Queries.GetAll;

public class GetAllProductsQueryValidator : AbstractValidator<GetAllProductsQuery>
{
    public GetAllProductsQueryValidator()
    {
        RuleFor(query => query.SortBy)
            .IsEnumName(typeof(ProductSoringOptions), false)
            .When(query => query.SortBy is not null);

        RuleFor(query => query.Direction)
            .IsEnumName(typeof(SortDirection), false)
            .When(query => query.Direction is not null);
    }
}