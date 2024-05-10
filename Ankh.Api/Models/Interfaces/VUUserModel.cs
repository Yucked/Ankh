using System.Text.Json.Serialization;
using Ankh.Api.Attributes;

namespace Ankh.Api.Models.Interfaces;

public class VUUser {
    [JsonPropertyNames("legacy_cid", "cid")]
    public ulong UserId { get; init; }
    
    [JsonPropertyNames("username", "avname")]
    public string Username { get; init; }
    
    [JsonPropertyNames("avatar_image", "avpic_url")]
    public string AvatarUrl { get; init; }
    
    [JsonPropertyName("registered")]
    public DateTimeOffset RegisteredOn { get; init; }
}