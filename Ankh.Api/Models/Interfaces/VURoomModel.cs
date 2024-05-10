using System.Text.Json.Serialization;

namespace Ankh.Api.Models.Interfaces;

/// <summary>
/// Can be used for public rooms
/// </summary>
public record VURoomModel {
    [JsonPropertyName("room_instance_id")]
    public string RoomId { get; init; }
    
    [JsonPropertyName("name")]
    public string Name { get; init; }
    
    [JsonPropertyName("is_ap")]
    public bool IsAP { get; init; }
    
    [JsonPropertyName("is_vip")]
    public bool IsVIP { get; init; }
}