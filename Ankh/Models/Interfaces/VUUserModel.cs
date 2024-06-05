using System.Text.Json.Serialization;

namespace Ankh.Models.Interfaces;

public class VUUser {
    [JsonPropertyName("legacy_cid, cid")]
    public int UserId { get; init; }
    
    [JsonPropertyName("username, avname")]
    public string Username { get; init; }
    
    [JsonPropertyName("avatar_image, avpic_url")]
    public string AvatarUrl { get; init; }
    
    [JsonPropertyName("registered")]
    public DateTimeOffset RegisteredOn { get; init; }
    
    [property: JsonPropertyName("gender")]
    public string Gender { get; init; }
    
    [JsonPropertyName("age")]
    public int Age { get; init; }
}