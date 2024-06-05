using System.Text.Json.Serialization;

namespace Ankh.Models.Interfaces;

/// <summary>
/// Can be used for public rooms
/// </summary>
public record VURoomModel {
    // TODO: instance id is used for getting room's information room_instance_id
    [JsonPropertyName("room_pid")]
    public long Id { get; init; }
    
    [JsonPropertyName("name")]
    public string Name { get; init; }
    
    [JsonPropertyName("is_ap")]
    public bool IsAP { get; init; }
    
    [JsonPropertyName("is_vip")]
    public bool IsVIP { get; init; }
}