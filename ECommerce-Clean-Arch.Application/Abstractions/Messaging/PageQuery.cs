using ECommerce_Clean_Arch.Application.Common.Models;

namespace ECommerce_Clean_Arch.Application.Abstractions.Messaging;

public interface IPageQuery<T> : IQuery<PaginatedList<T>>
{
    public int PageNo { get; init; }
    public int PageSize { get; init; }
    public string? SortBy { get; init; }
    public string? Direction { get; init; }
}

public abstract record DefaultPageQuery<T> : IPageQuery<T>
{
    public int PageNo { get; init; } = 1;
    public int PageSize { get; init; } = 5;
    public string? SortBy { get; init; } = "createdAt";
    public string? Direction { get; init; } = "desc";
}