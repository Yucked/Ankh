using System.Text.Json.Serialization;
using Ankh.Converters;
using Ankh.Converters.UserPropertiesConverters;
using Ankh.Models.Enums;

namespace Ankh.Models.Rework;

public record UserLogin : UserModelMinimal {
    public string Password { get; init; }
    public string Sauce { get; init; }
    public string SessionId { get; init; }
    public string SecurityKey { get; init; }
}

public record UserModelMinimal {
    public string Id { get; init; }
    public string Username { get; init; }
}

public record UserModelCommon : UserModelMinimal {
    [JsonPropertyName("registered"),
     JsonConverter(typeof(DateTimeConverter))]
    // TODO: Ignore the property in PHP or don't override
    public DateTimeOffset RegisteredOn { get; init; }
    
    [JsonPropertyName("gender"),
     JsonConverter(typeof(UserGenderConverter))]
    public string Gender { get; init; }
    
    [JsonConverter(typeof(NullToIntConverter))]
    public int Age { get; init; }
    
    [JsonPropertyName("badge_level")]
    public int BadgeLevel { get; init; }
    
    public string Tagline { get; init; }
    
    [JsonPropertyName("online")]
    public bool IsOnline { get; init; }
    
    public string ProfileImage { get; init; }
    
    [JsonConverter(typeof(AvailabilityConverter))]
    public Availability Availability { get; init; }
    
    [JsonConverter(typeof(UserInterestConverter))]
    public string Interests { get; init; }
    
    [JsonPropertyName("relationship_status"),
     JsonConverter(typeof(JsonNumberEnumConverter<RelationshipStatus>))]
    public RelationshipStatus Relationship { get; init; }
    
    [JsonPropertyName("orientation"),
     JsonConverter(typeof(JsonNumberEnumConverter<Orientation>))]
    public Orientation Orientation { get; init; }
    
    [JsonPropertyName("looking_for"),
     JsonConverter(typeof(JsonNumberEnumConverter<LookingFor>))]
    public LookingFor LookingFor { get; init; }
    
    public string State { get; init; }
    
    public string Country { get; init; }
}

public record UserModel : UserModelCommon {
    [JsonPropertyName("created"),
     JsonConverter(typeof(DateTimeConverter))]
    public DateTime CreatedOn { get; init; }
    
    [JsonPropertyName("display_name")]
    public string DisplayName { get; init; }
    
    [JsonPropertyName("is_vip")]
    public bool IsVip { get; init; }
    
    [JsonPropertyName("is_ap")]
    public bool IsAp { get; init; }
    
    [JsonPropertyName("is_creator")]
    public bool IsCreator { get; init; }
    
    [JsonPropertyName("is_adult")]
    public bool IsAdult { get; init; }
    
    [JsonPropertyName("is_ageverified")]
    public bool IsAgeverified { get; init; }
    
    [JsonPropertyName("is_staff")]
    public bool IsStaff { get; init; }
    
    [JsonPropertyName("is_greeter")]
    public bool IsGreeter { get; init; }
    
    [JsonPropertyName("greeter_score")]
    public int GreeterScore { get; init; }
    
    [JsonPropertyName("persona_type")]
    public int PersonaType { get; init; }
    
    [JsonPropertyName("is_on_hold")]
    public bool IsOnHold { get; init; }
    
    [JsonPropertyName("is_discussion_moderator")]
    public bool IsDiscussionModerator { get; init; }
    
    // Properties related to logged on user
    
    [JsonPropertyName("is_host"),
     JsonConverter(typeof(ValueToBoolConverter))]
    public bool IsHost { get; init; }
    
    [JsonPropertyName("is_current_user")]
    public bool IsCurrentUser { get; init; }
    
    [JsonPropertyName("ads_category")]
    public string AdsCategory { get; init; }
    
    [JsonPropertyName("ads_category_p")]
    public bool HasAdsCategory { get; init; }
    
    [JsonPropertyName("email")]
    public string Email { get; init; }
    
    [JsonPropertyName("is_email_verified")]
    public bool IsEmailVerified { get; init; }
    
    [JsonPropertyName("last_password_change")]
    public DateTimeOffset LastPasswordChange { get; init; }
    
    [JsonPropertyName("is_2fa_required")]
    public object Is2FaRequired { get; init; }
    
    [JsonPropertyName("has_nft")]
    public bool HasNft { get; init; }
    
    [JsonPropertyName("vip_tier")]
    public int VipTier { get; init; }
    
    [JsonPropertyName("vip_platform"),
     JsonConverter(typeof(NullToIntConverter))]
    public int VipPlatform { get; init; }
    
    [JsonPropertyName("has_legacy_vip")]
    public bool HasLegacyVip { get; init; }
    
    // PHP Properties
    [JsonPropertyName("is_buddy")]
    public bool IsBuddy { get; init; }
    
    [JsonPropertyName("is_friend")]
    public bool IsFriend { get; init; }
    
    [JsonPropertyName("is_qa")]
    public bool IsQa { get; init; }
    
    [JsonPropertyName("last_login"),
     JsonConverter(typeof(DateTimeConverter))]
    public DateTime LastLogin { get; init; }
    
    [JsonPropertyName("badge_count")]
    public int BadgeCount { get; init; }
    
    [JsonPropertyName("badge_area_html")]
    public string BadgeAreaHtml { get; init; }
    
    [JsonPropertyName("show_badgecount")]
    public bool ShowBadgecount { get; init; }
    
    [JsonPropertyName("show_flag_icon"),
     JsonConverter(typeof(ValueToBoolConverter))]
    public bool ShowFlagIcon { get; init; }
    
    [JsonPropertyName("show_flag_av"),
     JsonConverter(typeof(ValueToBoolConverter))]
    public bool ShowFlagAv { get; init; }
    
    [JsonPropertyName("show_message"),
     JsonConverter(typeof(ValueToBoolConverter))]
    public bool ShowMessage { get; init; }
    
    [JsonPropertyName("avpic_default"),
     JsonConverter(typeof(ValueToBoolConverter))]
    public bool IsDefaultProfileImage { get; init; }
    
    [JsonPropertyName("show_block")]
    public bool ShowBlock { get; init; }
    
    [JsonPropertyName("welcome_moderator_score")]
    public int WelcomeModeratorScore { get; init; }
    
    [JsonPropertyName("is_welcome_moderator")]
    public int IsWelcomeModerator { get; init; }
    
    [JsonPropertyName("public_rooms")]
    public object[] PublicRooms { get; init; }
    
    [JsonPropertyName("visible_albums")]
    public int VisibleAlbums { get; init; }
    
    public IReadOnlyCollection<BadgeModelProfile> ProfileBadges { get; init; }
}