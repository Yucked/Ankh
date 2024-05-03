using System.Text.Json.Serialization;
using Ankh.Api.Converters;

namespace Ankh.Api.Models;

public interface IRestModel;

public record RestModel {
    [JsonPropertyName("status")]
    public string Status { get; init; }
    
    [JsonPropertyName("id")]
    public string Id { get; init; }
    
    [JsonPropertyName("denormalized"), JsonConverter(typeof(RestModelConverter))]
    public IRestModel Data { get; set; }
    
    public IDictionary<string, string> Relations { get; set; }
}