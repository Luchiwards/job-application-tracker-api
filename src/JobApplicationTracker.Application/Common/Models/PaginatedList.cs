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
        if (page <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(page));
        }

        if (pageSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize));
        }

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

    public static PaginatedList<T> Empty(int page, int pageSize) =>
        new(Array.Empty<T>(), 0, page, pageSize);

    public PaginatedList<TResult> Map<TResult>(Func<T, TResult> converter)
    {
        var mapped = Items.Select(converter).ToList();
        return new PaginatedList<TResult>(mapped, TotalCount, Page, PageSize);
    }
}

