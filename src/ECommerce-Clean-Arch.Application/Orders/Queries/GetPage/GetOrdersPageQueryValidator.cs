namespace ECommerce_Clean_Arch.Application.Orders.Queries.GetPage;

public sealed class GetOrdersPageQueryValidator : AbstractValidator<GetOrdersPageQuery>
{
    public GetOrdersPageQueryValidator()
    {
        RuleFor(query => query.PageSize).InclusiveBetween(1, 100);

        RuleFor(query => query.PageNo).GreaterThan(0);


        RuleFor(query => query.SortBy!.Trim())
            .IsEnumName(typeof(OrderSortingOptions), false)
            .When(query => query.SortBy is not null);

        RuleFor(query => query.Direction!.Trim())
            .IsEnumName(typeof(SortDirection), false)
            .When(query => query.Direction is not null);
    }
}