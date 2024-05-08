using System.Text.Json;
using System.Text.Json.Serialization;
using Ankh.Api.Models;

namespace Ankh.Api.Converters;

public class RestModelConverter : JsonConverter<IRestModel> {
    public override IRestModel? Read(ref Utf8JsonReader reader, Type typeToConvert,
                                     JsonSerializerOptions options) {
        if (!JsonDocument.TryParseValue(ref reader, out var document)) {
            return default;
        }
        
        var id = document.RootElement.EnumerateObject().First().Name;
        var data = document.RootElement
            .GetProperty(id)
            .GetProperty("data");
        IRestModel result = id switch {
            _ when id.Contains("product/product-") => data.Deserialize<ProductModel>(),
            _ when id.Contains("user/user-")       => data.Deserialize<UserModel>(),
            _ when id.Contains("room/room-")       => data.Deserialize<RoomModel>(),
        };
        
        return result;
    }
    
    public override void Write(Utf8JsonWriter writer, IRestModel value, JsonSerializerOptions options) {
        throw new NotImplementedException();
    }
}