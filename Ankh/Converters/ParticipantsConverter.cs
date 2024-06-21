using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ankh.Converters;

public sealed class ParticipantsConverter : JsonConverter<IDictionary<string, DateTime>> {
    public override IDictionary<string, DateTime> Read(ref Utf8JsonReader reader, Type typeToConvert,
                                                       JsonSerializerOptions options) {
        var document = JsonDocument.ParseValue(ref reader).RootElement;
        if (document.ValueKind == JsonValueKind.Array) {
            return new Dictionary<string, DateTime>();
        }
        
        return document.EnumerateObject()
            .Select(x => (x.Name, DateTime.Now))
            .ToDictionary(x => x.Name, y => y.Now);
    }
    
    public override void Write(Utf8JsonWriter writer, IDictionary<string, DateTime> value,
                               JsonSerializerOptions options) {
        writer.WriteStringValue(JsonSerializer.Serialize(value));
    }
}