using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ankh.Converters;

public class StringToBoolConverter : JsonConverter<bool> {
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        var str = reader.GetString();
        return !string.IsNullOrWhiteSpace(str) && bool.Parse(str);
    }
    
    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options) {
        writer.WriteBooleanValue(value);
    }
}