﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ankh.Converters;

internal sealed class IntToStringConverter : JsonConverter<string> {
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        return $"{reader.GetInt32()}";
    }
    
    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options) {
        writer.WriteStringValue(value);
    }
}