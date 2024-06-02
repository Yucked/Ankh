using System.Text.Json.Serialization;

namespace Ankh.Api.Models.Rest;

public record RestProfileModel(
    [property: JsonPropertyName("image")]
    string Image,
    [property: JsonPropertyName("title")]
    string Title,
    [property: JsonPropertyName("type")]
    string Type,
    [property: JsonPropertyName("is_persona")]
    bool IsPersona,
    [property: JsonPropertyName("reportable")]
    bool IsReportable,
    [property: JsonPropertyName("online")]
    bool IsOnline,
    [property: JsonPropertyName("avatar_name")]
    string Username,
    [property: JsonPropertyName("approx_follower_count")]
    int Followers,
    [property: JsonPropertyName("approx_following_count")]
    int Following) : IRestModel;