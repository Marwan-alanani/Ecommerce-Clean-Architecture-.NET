namespace ECommerce_Clean_Arch.Application.Common.Models;

public static class MappingExtensions
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable,
        int pageNo,
        int pageSize,
        CancellationToken cancellationToken = default
    )
        where TDestination : class =>
        PaginatedList<TDestination>
            .CreateAsync(
                queryable,
                pageNo,
                pageSize,
                cancellationToken);
}