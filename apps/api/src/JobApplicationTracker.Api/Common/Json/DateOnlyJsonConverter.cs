using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JobApplicationTracker.Api.Common.Json;

public sealed class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string Format = "yyyy-MM-dd";

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

    public override void Write(
        Utf8JsonWriter writer,
        DateOnly value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
    }
}


