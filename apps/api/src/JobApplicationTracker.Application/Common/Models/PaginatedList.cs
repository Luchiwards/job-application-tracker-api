using System;
using System.Collections.Generic;
using System.Linq;

namespace JobApplicationTracker.Application.Common.Models;

/// <summary>
/// Represents an immutable paginated slice of a larger result set.
/// </summary>
/// <typeparam name="T">Type of the items contained in the page.</typeparam>
public sealed class PaginatedList<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PaginatedList{T}"/> class.
    /// </summary>
    /// <param name="items">Items belonging to the requested page.</param>
    /// <param name="totalCount">Total number of items available across all pages.</param>
    /// <param name="page">Current page number (1-based).</param>
    /// <param name="pageSize">Number of items in each page.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="page"/> or <paramref name="pageSize"/> is less than or equal to zero.
    /// </exception>
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

    /// <summary>
    /// Gets the items contained within the current page.
    /// </summary>
    public IReadOnlyList<T> Items { get; }

    /// <summary>
    /// Gets the total number of items available across all pages.
    /// </summary>
    public int TotalCount { get; }

    /// <summary>
    /// Gets the current page number (1-based).
    /// </summary>
    public int Page { get; }

    /// <summary>
    /// Gets the number of items contained in each page.
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// Gets the total number of pages available.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    /// <summary>
    /// Gets a value indicating whether a page exists before the current page.
    /// </summary>
    public bool HasPreviousPage => Page > 1;

    /// <summary>
    /// Gets a value indicating whether a page exists after the current page.
    /// </summary>
    public bool HasNextPage => Page < TotalPages;

    /// <summary>
    /// Projects the items in the current page to another type while preserving pagination metadata.
    /// </summary>
    /// <typeparam name="TResult">Type of the mapped items.</typeparam>
    /// <param name="selector">Projection applied to each item in the current page.</param>
    /// <returns>A paginated list whose items are the result of invoking the selector.</returns>
    public PaginatedList<TResult> Map<TResult>(Func<T, TResult> selector)
    {
        var mapped = Items.Select(selector).ToList();
        return new PaginatedList<TResult>(mapped, TotalCount, Page, PageSize);
    }
}

/// <summary>
/// Factory helpers for constructing <see cref="PaginatedList{T}"/> instances.
/// </summary>
public static class PaginatedList
{
    /// <summary>
    /// Creates a paginated list by mapping a source collection to a different representation.
    /// </summary>
    /// <typeparam name="TSource">Type of the source items.</typeparam>
    /// <typeparam name="TResult">Type of the projected items.</typeparam>
    /// <param name="items">Source items belonging to the requested page.</param>
    /// <param name="totalCount">Total number of items available across all pages.</param>
    /// <param name="page">Current page number (1-based).</param>
    /// <param name="pageSize">Number of items in each page.</param>
    /// <param name="selector">Projection applied to each source item.</param>
    /// <returns>A new paginated list containing the mapped items.</returns>
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

