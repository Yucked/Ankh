using System.Text.Json;
using System.Text.Json.Serialization;
using Ankh.Api.Models;

namespace Ankh.Api.Converters;

public sealed class ProductRatingConverter : JsonConverter<ProductRating> {
    public override ProductRating Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        return (ProductRating)reader.ValueSpan[0];
    }
    
    public override void Write(Utf8JsonWriter writer, ProductRating value, JsonSerializerOptions options) {
        throw new NotImplementedException();
    }
}