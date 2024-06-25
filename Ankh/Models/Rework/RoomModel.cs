using System.Text.Json.Serialization;
using Ankh.Converters;

namespace Ankh.Models.Rework;

// Minimal information returned from API endpoint find_locations PHP
public record RoomModelMinimal {
    [JsonPropertyName("id")]
    public string Id { get; init; }
    
    [JsonPropertyName("name")]
    public string Name { get; init; }
    
    [JsonPropertyName("isAP")]
    public bool IsAp { get; init; }
    
    [JsonPropertyName("isVIP")]
    public bool IsVip { get; init; }
}

// Common properties between PHP and Rest
public record RoomModelCommon : RoomModelMinimal {
    [JsonPropertyName("description")]
    public string Description { get; init; }
    
    // customers_id
    [JsonPropertyName("ownerId")]
    public long OwnerId { get; init; }
    
    // rest owner_avatarname, php customers_avatar_name
    [JsonPropertyName("ownerUsername")]
    public string OwnerUsername { get; init; }
    
    // rest capacity, php max_users 
    [JsonPropertyName("capacity")]
    public int Capacity { get; init; }
    
    [JsonPropertyName("image_url")]
    public string RoomImageUrl { get; init; }
    
    [JsonPropertyName("rating")]
    public float Ratings { get; init; }
    
    [JsonPropertyName("language")]
    public string Language { get; init; }
    
    [JsonPropertyName("is_friends_only")]
    public bool IsFriendsOnly { get; set; }
    
    // rest is_age_verified, php is_age_verified_only
    [JsonPropertyName("is_age_verified")]
    public bool IsAgeVerified { get; set; }
    
    [JsonPropertyName("is_non_guest_only")]
    public bool IsNonGuestOnly { get; set; }
}

// Rest API 
public record RoomModel : RoomModelCommon {
    [JsonPropertyName("type")]
    public string Type { get; set; }
    
    [JsonPropertyName("privacy")]
    public string Privacy { get; set; }
    
    [JsonPropertyName("auto_boot_when_owner_leaves")]
    public bool AutoBootWhenOwnerLeaves { get; set; }
    
    [JsonPropertyName("auto_boot_timeout")]
    public long AutoBootTimeout { get; set; }
    
    [JsonPropertyName("themed_image_url")]
    public string ThemedImageUrl { get; set; }
    
    [JsonPropertyName("occupancy"), JsonConverter(typeof(NullToIntConverter))]
    public int Occupancy { get; set; }
    
    [JsonPropertyName("language_code")]
    public string LanguageCode { get; set; }
    
    [JsonPropertyName("content_rating")]
    public string ContentRating { get; set; }
    
    [JsonPropertyName("has_plus_badge")]
    public bool HasPlusBadge { get; set; }
    
    [JsonPropertyName("is_plus_badge_compatible")]
    public bool IsPlusBadgeCompatible { get; set; }
    
    [JsonPropertyName("has_audio")]
    public bool HasAudio { get; set; }
    
    [JsonPropertyName("supports_youtube")]
    public bool SupportsYoutube { get; set; }
    
    [JsonPropertyName("supports_audience")]
    public bool SupportsAudience { get; set; }
    
    [JsonPropertyName("mimic_chat_room")]
    public bool MimicChatRoom { get; set; }
    
    [JsonPropertyName("rendered_image")]
    public string RenderedImage { get; set; }
    
    [JsonPropertyName("join_room_url")]
    public string JoinRoomUrl { get; set; }
    
    [JsonPropertyName("customers_room_id")]
    public int CustomersRoomId { get; set; }
    
    [JsonPropertyName("is_expired")]
    public bool IsExpired { get; set; }
    
    // PHP API 
    
    [JsonPropertyName("permissions")]
    public int Permissions { get; set; }
    
    [JsonPropertyName("url_token")]
    public string UrlToken { get; set; }
    
    [JsonPropertyName("visitor_count")]
    public int VisitorCount { get; set; }
    
    [JsonPropertyName("is_qa")]
    public bool IsQa { get; set; }
    
    [JsonPropertyName("whitelist_rating")]
    public int WhitelistRating { get; set; }
    
    [JsonPropertyName("experiences_id")]
    public string ExperiencesId { get; set; }
    
    [JsonPropertyName("mogile_key")]
    public string MogileKey { get; set; }
    
    [JsonPropertyName("mogile_domain")]
    public string MogileDomain { get; set; }
    
    [JsonPropertyName("expired")]
    public object Expired { get; set; }
    
    [JsonPropertyName("room_owners")]
    public object[] RoomOwners { get; set; }
    
    [JsonPropertyName("can_join_when_closed")]
    public bool CanJoinWhenClosed { get; set; }
    
    [JsonPropertyName("allow_room_shell_change")]
    public bool AllowRoomShellChange { get; set; }
    
    [JsonPropertyName("allow_ap_products")]
    public bool AllowApProducts { get; set; }
    
    [JsonPropertyName("room_shell_owner_id")]
    public int RoomShellOwnerId { get; set; }
    
    [JsonPropertyName("visitors_count")]
    public int VisitorsCount { get; set; }
    
    [JsonPropertyName("boot_count")]
    public int BootCount { get; set; }
    
    [JsonPropertyName("auto_innocent_count")]
    public int AutoInnocentCount { get; set; }
    
    [JsonPropertyName("is_grey_area")]
    public bool IsGreyArea { get; set; }
    
    [JsonPropertyName("is_greylisted")]
    public bool IsGreylisted { get; set; }
    
    [JsonPropertyName("last_modified"), JsonConverter(typeof(DateTimeConverter))]
    public DateTimeOffset LastModified { get; set; }
    
    [JsonPropertyName("participants"), JsonConverter(typeof(ParticipantsConverter))]
    public IDictionary<string, DateTimeOffset> Participants { get; set; }
    
    [JsonPropertyName("room_download_size")]
    public long RoomDownloadSize { get; set; }
}