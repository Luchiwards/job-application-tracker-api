using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JobApplicationTracker.Api.Common.Json;

/// <summary>
/// Converts <see cref="DateOnly"/> values to and from their ISO-8601 string representation for JSON payloads.
/// </summary>
public sealed class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string Format = "yyyy-MM-dd";

    /// <inheritdoc />
    public override DateOnly Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (!DateOnly.TryParse(value, out var date))
        {
            throw new JsonException($"Unable to convert \"{value}\" to DateOnly.");
        }

        return date;
    }

    /// <inheritdoc />
    public override void Write(
        Utf8JsonWriter writer,
        DateOnly value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
    }
}


