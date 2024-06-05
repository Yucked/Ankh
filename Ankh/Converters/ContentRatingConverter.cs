using System.Text.Json;
using System.Text.Json.Serialization;
using Ankh.Models.Enums;

namespace Ankh.Converters;

public sealed class ContentRatingConverter : JsonConverter<ContentRating> {
    public override ContentRating Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        return (ContentRating)reader.ValueSpan[0];
    }
    
    public override void Write(Utf8JsonWriter writer, ContentRating value, JsonSerializerOptions options) {
        throw new NotImplementedException();
    }
}