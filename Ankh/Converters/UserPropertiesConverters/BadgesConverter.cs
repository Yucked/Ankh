using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ankh.Models.Rework;

namespace Ankh.Converters.UserPropertiesConverters;

internal sealed class BadgesConverter : JsonConverter<IReadOnlyCollection<BadgeModelProfile>> {
    public override IReadOnlyCollection<BadgeModelProfile> Read(ref Utf8JsonReader reader, Type typeToConvert,
                                                                JsonSerializerOptions options) {
        var document = JsonDocument.ParseValue(ref reader).RootElement;
        if (document.ValueKind == JsonValueKind.Array) {
            return new Collection<BadgeModelProfile>();
        }
        
        return document
            .EnumerateObject()
            .Select(x => x.Value.Deserialize<BadgeModelProfile>())
            .ToArray()!;
    }
    
    public override void Write(Utf8JsonWriter writer, IReadOnlyCollection<BadgeModelProfile> value,
                               JsonSerializerOptions options) {
        writer.WriteStringValue(JsonSerializer.Serialize(value));
    }
}