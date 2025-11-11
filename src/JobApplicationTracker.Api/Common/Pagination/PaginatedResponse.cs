using System;
using System.Collections.Generic;
using JobApplicationTracker.Application.Common.Models;

namespace JobApplicationTracker.Api.Common.Pagination;

public sealed record PaginatedResponse<T>(
    IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages,
    bool HasPreviousPage,
    bool HasNextPage);

public static class PaginatedResponseFactory
{
    public static PaginatedResponse<TDestination> From<TSource, TDestination>(
        PaginatedList<TSource> source,
        Func<TSource, TDestination> map)
    {
        return From(source.Map(map));
    }

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

