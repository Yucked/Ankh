using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ankh.Converters.UserPropertiesConverters;

internal sealed class UserGenderConverter : JsonConverter<string> {
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        var str = reader.GetString();
        return str switch {
            ""   => string.Empty,
            null => string.Empty,
            "m"  => "Male",
            "f"  => "Female",
            _    => str
        };
    }
    
    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options) {
        writer.WriteStringValue(value);
    }
}