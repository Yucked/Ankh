using System.Text.Json.Serialization;
using Ankh.Api.Models;

namespace Ankh.Api;

[JsonDerivedType(typeof(ProductModel))]
public class RestResponse {
    [JsonPropertyName("status")]
    public string Status { get; init; }
    
    [JsonPropertyName("id")]
    public string Id { get; init; }
    
}

public record RestHttpModel {
    
}