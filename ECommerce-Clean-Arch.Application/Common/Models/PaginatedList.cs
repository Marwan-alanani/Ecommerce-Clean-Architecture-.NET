using Microsoft.EntityFrameworkCore;

namespace ECommerce_Clean_Arch.Application.Common.Models;

public class PaginatedList<T>
{
    private PaginatedList(
        IReadOnlyCollection<T> items,
        int pageNumber,
        int pageSize,
        int totalCount
    )
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public int PageNumber { get; }
    public int PageSize { get; }
    public IReadOnlyCollection<T> Items { get; }
    public int TotalCount { get; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNext => PageNumber + 1 < TotalPages;
    public bool HasPrevious => PageNumber > 1;

    public static async Task<PaginatedList<T>> CreateAsync(
        IQueryable<T> source,
        int pageNo,
        int pageSize,
        CancellationToken cancellationToken = default
    )
    {
        var totalCount = await source.CountAsync(cancellationToken);
        var items = await source
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);


        return new PaginatedList<T>(
            items,
            pageNo,
            pageSize,
            totalCount
        );
    }
}