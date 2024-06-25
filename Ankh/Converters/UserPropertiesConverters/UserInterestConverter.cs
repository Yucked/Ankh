using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ankh.Converters.UserPropertiesConverters;

internal sealed class UserInterestConverter : JsonConverter<string> {
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        return reader.TokenType == JsonTokenType.StartObject
            ? JsonDocument.Parse(reader.ValueSequence).RootElement
                .GetProperty("full_text_string")
                .GetProperty("raw_tag")
                .GetString()!
            : reader.GetString()!;
    }
    
    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options) {
        writer.WriteStringValue(value);
    }
}