using System;
using System.Collections.Generic;
using System.Linq;

namespace JobApplicationTracker.Application.Common.Models;

public sealed class PaginatedList<T>
{
    public PaginatedList(
        IReadOnlyList<T> items,
        int totalCount,
        int page,
        int pageSize)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(page, 0);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(pageSize, 0);

        Items = items;
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
    }

    public IReadOnlyList<T> Items { get; }

    public int TotalCount { get; }

    public int Page { get; }

    public int PageSize { get; }

    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public bool HasPreviousPage => Page > 1;

    public bool HasNextPage => Page < TotalPages;

    public PaginatedList<TResult> Map<TResult>(Func<T, TResult> selector)
    {
        var mapped = Items.Select(selector).ToList();
        return new PaginatedList<TResult>(mapped, TotalCount, Page, PageSize);
    }
}

public static class PaginatedList
{
    public static PaginatedList<TResult> Create<TSource, TResult>(
        IReadOnlyList<TSource> items,
        int totalCount,
        int page,
        int pageSize,
        Func<TSource, TResult> selector)
    {
        var mapped = items.Select(selector).ToList();
        return new PaginatedList<TResult>(mapped, totalCount, page, pageSize);
    }
}

