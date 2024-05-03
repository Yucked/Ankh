using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ankh.Api.Models;

namespace Ankh.Api.Converters;

public class RestModelConverter : JsonConverter<IRestModel> {
    public override IRestModel? Read(ref Utf8JsonReader reader, Type typeToConvert,
                                     JsonSerializerOptions options) {
        reader.Read();
        var id = Encoding.UTF8.GetString(reader.ValueSpan);
        
        if (!JsonDocument.TryParseValue(ref reader, out var document)) {
            return default;
        }
        
        var result = id switch {
            _ when id.Contains("product/product-") => document.RootElement
                .GetProperty("data")
                .Deserialize<ProductModel>()
        };
        
        return result;
    }
    
    public override void Write(Utf8JsonWriter writer, IRestModel value, JsonSerializerOptions options) {
        throw new NotImplementedException();
    }
}