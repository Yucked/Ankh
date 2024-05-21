using System.Text.Json.Serialization;
using Ankh.Api.Models.Interfaces;

namespace Ankh.Api.Models.PHP;

public record PHPRoomModel : VURoomModel{
    [JsonPropertyName("room_pid")]
    public long RoomPid { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("lieutenants")]
    public object[] Lieutenants { get; set; }
    
    [JsonPropertyName("room_owners")]
    public long[] RoomOwners { get; set; }
    
    [JsonPropertyName("allow_ap_products")]
    public bool AllowApProducts { get; set; }
    
    [JsonPropertyName("allow_room_shell_change")]
    public bool AllowRoomShellChange { get; set; }
    
    [JsonPropertyName("allow_load_new_room")]
    public bool AllowLoadNewRoom { get; set; }
    
    [JsonPropertyName("max_users")]
    public long MaxUsers { get; set; }
    
    [JsonPropertyName("language")]
    public object Language { get; set; }
    
    [JsonPropertyName("is_ap")]
    public bool IsAp { get; set; }
    
    [JsonPropertyName("is_vip")]
    public bool IsVip { get; set; }
    
    [JsonPropertyName("allow_games")]
    public bool AllowGames { get; set; }
    
    [JsonPropertyName("is_favorite")]
    public bool IsFavorite { get; set; }
    
    [JsonPropertyName("show_favorite")]
    public bool ShowFavorite { get; set; }
    
    [JsonPropertyName("show_ratings")]
    public bool ShowRatings { get; set; }
    
    [JsonPropertyName("enable_flagging")]
    public bool EnableFlagging { get; set; }
    
    [JsonPropertyName("private")]
    public bool Private { get; set; }
    
    [JsonPropertyName("auto_boot_when_owner_leaves")]
    public bool AutoBootWhenOwnerLeaves { get; set; }
    
    [JsonPropertyName("participants")]
    public object[] Participants { get; set; }
    
    [JsonPropertyName("customers_avatar_name")]
    public string CustomersAvatarName { get; set; }
    
    [JsonPropertyName("room_download_size")]
    public long RoomDownloadSize { get; set; }
    
    [JsonPropertyName("rating")]
    public long Rating { get; set; }
    
    [JsonPropertyName("room_instance_id")]
    public string RoomInstanceId { get; set; }
    
    [JsonPropertyName("customers_id")]
    public long CustomersId { get; set; }
}