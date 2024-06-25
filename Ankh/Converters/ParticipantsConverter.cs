using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ankh.Converters;

public sealed class ParticipantsConverter : JsonConverter<IDictionary<string, DateTimeOffset>> {
    public override IDictionary<string, DateTimeOffset> Read(ref Utf8JsonReader reader, Type typeToConvert,
                                                             JsonSerializerOptions options) {
        var document = JsonDocument.ParseValue(ref reader).RootElement;
        if (document.ValueKind == JsonValueKind.Array) {
            return new Dictionary<string, DateTimeOffset>();
        }
        
        return document.EnumerateObject()
            .Select(x => (x.Name, DateTimeOffset.UtcNow))
            .ToDictionary(x => x.Name, y => y.UtcNow);
    }
    
    public override void Write(Utf8JsonWriter writer, IDictionary<string, DateTimeOffset> value,
                               JsonSerializerOptions options) {
        writer.WriteStringValue(JsonSerializer.Serialize(value));
    }
}