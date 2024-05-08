using System.Text.Json.Serialization;
using Ankh.Api.Converters;
using Ankh.Api.Models.Enums;

namespace Ankh.Api.Models;

public record UserModel : IRestModel {
    [JsonPropertyName("created")]
    public DateTimeOffset Created { get; set; }
    
    [JsonPropertyName("registered")]
    public long Registered { get; set; }
    
    [JsonPropertyName("gender")]
    public string Gender { get; set; }
    
    [JsonPropertyName("display_name")]
    public string DisplayName { get; set; }
    
    [JsonPropertyName("age"), JsonConverter(typeof(NullIntConverter))]
    public int Age { get; set; }
    
    [JsonPropertyName("country")]
    public string Country { get; set; }
    
    [JsonPropertyName("state")]
    public object State { get; set; }
    
    [JsonPropertyName("avatar_image")]
    public Uri AvatarImage { get; set; }
    
    [JsonPropertyName("avatar_portrait_image")]
    public Uri AvatarPortraitImage { get; set; }
    
    [JsonPropertyName("is_vip")]
    public bool IsVip { get; set; }
    
    [JsonPropertyName("is_ap")]
    public bool HasAccessPass { get; set; }
    
    [JsonPropertyName("is_creator")]
    public bool IsCreator { get; set; }
    
    [JsonPropertyName("is_adult")]
    public bool IsAdult { get; set; }
    
    [JsonPropertyName("is_ageverified")]
    public bool IsAgeverified { get; set; }
    
    [JsonPropertyName("is_staff")]
    public bool IsStaff { get; set; }
    
    [JsonPropertyName("is_greeter")]
    public bool IsGreeter { get; set; }
    
    [JsonPropertyName("greeter_score")]
    public int GreeterScore { get; set; }
    
    [JsonPropertyName("badge_level")]
    public int BadgeLevel { get; set; }
    
    [JsonPropertyName("username")]
    public string Username { get; set; }
    
    [JsonPropertyName("relationship_status"), JsonConverter(typeof(JsonNumberEnumConverter<RelationshipStatus>))]
    public RelationshipStatus RelationshipStatus { get; set; }
    
    [JsonPropertyName("orientation")]
    public int Orientation { get; set; }
    
    [JsonPropertyName("looking_for"), JsonConverter(typeof(JsonNumberEnumConverter<LookingFor>))]
    public LookingFor LookingFor { get; set; }
    
    [JsonPropertyName("interests")]
    public string Interests { get; set; }
    
    [JsonPropertyName("legacy_cid")]
    public long LegacyCid { get; set; }
    
    [JsonPropertyName("persona_type")]
    public long PersonaType { get; set; }
    
    [JsonPropertyName("availability"), JsonConverter(typeof(AvailabilityConverter))]
    public Availability Availability { get; set; }
    
    [JsonPropertyName("is_on_hold")]
    public bool IsOnHold { get; set; }
    
    [JsonPropertyName("is_discussion_moderator")]
    public bool IsDiscussionModerator { get; set; }
    
    [JsonPropertyName("online")]
    public bool Online { get; set; }
    
    [JsonPropertyName("tagline")]
    public string Tagline { get; set; }
    
    [JsonPropertyName("thumbnail_url")]
    public Uri ThumbnailUrl { get; set; }
    
    [JsonPropertyName("is_host"), JsonConverter(typeof(IntToBoolConverter))]
    public bool IsHost { get; set; }
    
    [JsonPropertyName("is_current_user")]
    public bool IsCurrentUser { get; set; }
    
    [JsonPropertyName("ads_category")]
    public string AdsCategory { get; set; }
    
    [JsonPropertyName("ads_category_p")]
    public bool AdsCategoryP { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    [JsonPropertyName("is_email_verified")]
    public bool IsEmailVerified { get; set; }
    
    [JsonPropertyName("last_password_change")]
    public DateTimeOffset LastPasswordChange { get; set; }
    
    [JsonPropertyName("is_2fa_required")]
    public object Is2FARequired { get; set; }
    
    [JsonPropertyName("has_nft")]
    public bool HasNft { get; set; }
    
    [JsonPropertyName("vip_tier")]
    public int VipTier { get; set; }
    
    [JsonPropertyName("vip_platform")]
    public int VipPlatform { get; set; }
    
    [JsonPropertyName("has_legacy_vip")]
    public bool HasLegacyVip { get; set; }
}