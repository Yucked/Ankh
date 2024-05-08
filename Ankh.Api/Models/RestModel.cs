using System.Text.Json.Serialization;
using Ankh.Api.Converters;

namespace Ankh.Api.Models;

public interface IRestModel;

public record RestModel(
    [property: JsonPropertyName("status")]
    string Status,
    [property: JsonPropertyName("id")]
    string Id,
    [property: JsonPropertyName("denormalized"), JsonConverter(typeof(RestModelConverter))]
    IRestModel Data
);