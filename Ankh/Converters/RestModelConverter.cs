﻿using System.Text.Json;
using System.Text.Json.Serialization;
using Ankh.Api.Models;
using Ankh.Models.Rest;

namespace Ankh.Converters;

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
            _ when id.Contains("product/product-") => data.Deserialize<RestProductModel>(),
            _ when id.Contains("user/user-")       => data.Deserialize<RestUserModel>(),
            _ when id.Contains("room/room-")       => data.Deserialize<RestRoomModel>(),
            _ when id.Contains("profile/profile")  => data.Deserialize<RestProfileModel>(),
            _ when id.Contains("account/account")  => data.Deserialize<RestAccountModel>(),
            _ when id.Contains("outfit/outfit")    => data.Deserialize<RestOutfitModel>()
        };
        
        return result;
    }
    
    public override void Write(Utf8JsonWriter writer, IRestModel value, JsonSerializerOptions options) {
        throw new NotImplementedException();
    }
}