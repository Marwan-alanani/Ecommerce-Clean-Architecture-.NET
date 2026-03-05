using ECommerce_Clean_Arch.Application.Common.Models;

using FluentValidation;

namespace ECommerce_Clean_Arch.Application.Products.Queries.GetAll;

public class GetAllProductsQueryValidator : AbstractValidator<GetAllProductsQuery>
{
    public GetAllProductsQueryValidator()
    {
        RuleFor(query => query.PageSize).InclusiveBetween(1, 100);

        RuleFor(query => query.PageNo).GreaterThan(0);


        RuleFor(query => query.SortBy!.Trim())
            .IsEnumName(typeof(ProductSortingOptions), false)
            .When(query => query.SortBy is not null);

        RuleFor(query => query.Direction!.Trim())
            .IsEnumName(typeof(SortDirection), false)
            .When(query => query.Direction is not null);
    }
}