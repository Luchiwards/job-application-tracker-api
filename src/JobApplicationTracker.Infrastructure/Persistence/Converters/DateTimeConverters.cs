using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace JobApplicationTracker.Infrastructure.Persistence.Converters;

public static class DateTimeConverters
{
    public static readonly ValueConverter<DateOnly, DateTime> DateOnlyToDateTime =
        new(
            dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue),
            dateTime => DateOnly.FromDateTime(dateTime));

    public static readonly ValueConverter<DateTimeOffset, DateTimeOffset> UtcDateTimeOffset =
        new(
            dateTimeOffset => dateTimeOffset,
            dateTimeOffset => dateTimeOffset.ToUniversalTime());
}


