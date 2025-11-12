using System;
using System.Collections.Generic;
using JobApplicationTracker.Application.Common.Models;

namespace JobApplicationTracker.Api.Common.Pagination;

/// <summary>
/// Represents a paginated collection of items returned from the API.
/// </summary>
/// <typeparam name="T">Type of the items contained in the page.</typeparam>
/// <param name="Items">The items belonging to the current page.</param>
/// <param name="Page">The current page number (1-based).</param>
/// <param name="PageSize">The number of items included in each page.</param>
/// <param name="TotalCount">The total number of items available across all pages.</param>
/// <param name="TotalPages">The total number of pages available.</param>
/// <param name="HasPreviousPage">Indicates whether a page exists before the current page.</param>
/// <param name="HasNextPage">Indicates whether a page exists after the current page.</param>
public sealed record PaginatedResponse<T>(
    IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages,
    bool HasPreviousPage,
    bool HasNextPage);

/// <summary>
/// Factory helpers for converting application pagination models into API responses.
/// </summary>
public static class PaginatedResponseFactory
{
    /// <summary>
    /// Creates a paginated response by mapping each item in the source page to another representation.
    /// </summary>
    /// <typeparam name="TSource">Type of the source items.</typeparam>
    /// <typeparam name="TDestination">Type of the destination items.</typeparam>
    /// <param name="source">Source pagination model.</param>
    /// <param name="map">Mapping function applied to each item in the source page.</param>
    /// <returns>A paginated response containing the mapped items.</returns>
    public static PaginatedResponse<TDestination> From<TSource, TDestination>(
        PaginatedList<TSource> source,
        Func<TSource, TDestination> map)
    {
        return From(source.Map(map));
    }

    /// <summary>
    /// Creates a paginated response directly from an application pagination model.
    /// </summary>
    /// <typeparam name="TDestination">Type of the items contained in the pagination model.</typeparam>
    /// <param name="source">Source pagination model.</param>
    /// <returns>A paginated response mirroring the supplied pagination model.</returns>
    public static PaginatedResponse<TDestination> From<TDestination>(
        PaginatedList<TDestination> source)
    {
        return new PaginatedResponse<TDestination>(
            source.Items,
            source.Page,
            source.PageSize,
            source.TotalCount,
            source.TotalPages,
            source.HasPreviousPage,
            source.HasNextPage);
    }
}

