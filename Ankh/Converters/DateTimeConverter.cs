using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ankh.Converters;

public sealed class DateTimeConverter : JsonConverter<DateTime> {
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        try {
            return reader.TokenType == JsonTokenType.Number
                ? DateTimeOffset.FromUnixTimeSeconds(reader.GetInt32()).DateTime
                : DateTime.Parse(reader.GetString()!);
        }
        catch {
            return DateTime.MinValue;
        }
    }
    
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) {
        writer.WriteStringValue(value);
    }
}