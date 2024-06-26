using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ankh.Converters;

public sealed class DateTimeConverter : JsonConverter<DateTimeOffset> {
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        try {
            return reader.TokenType == JsonTokenType.Number
                ? DateTimeOffset.FromUnixTimeSeconds(reader.GetInt32()).DateTime
                : DateTime.Parse(reader.GetString()!);
        }
        catch {
            return DateTime.MinValue;
        }
    }
    
    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options) {
        writer.WriteStringValue(value);
    }
}