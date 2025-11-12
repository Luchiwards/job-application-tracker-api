using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace JobApplicationTracker.Infrastructure.Persistence.Converters;

/// <summary>
/// Provides reusable EF Core value converters for date and time types.
/// </summary>
public static class DateTimeConverters
{
    /// <summary>
    /// Converts between <see cref="DateOnly"/> and <see cref="DateTime"/> using UTC midnight.
    /// </summary>
    public static readonly ValueConverter<DateOnly, DateTime> DateOnlyToDateTime =
        new(
            dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue),
            dateTime => DateOnly.FromDateTime(dateTime));

    /// <summary>
    /// Ensures <see cref="DateTimeOffset"/> values are stored in UTC.
    /// </summary>
    public static readonly ValueConverter<DateTimeOffset, DateTimeOffset> UtcDateTimeOffset =
        new(
            dateTimeOffset => dateTimeOffset,
            dateTimeOffset => dateTimeOffset.ToUniversalTime());
}


