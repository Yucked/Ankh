using System.Text.Json.Serialization;
using Ankh.Converters;
using Ankh.Converters.UserPropertiesConverters;
using Ankh.Models.Enums;

namespace Ankh.Models.Rework;

public record UserLogin : BasicUserModel {
    public string Password { get; init; }
    public string Sauce { get; init; }
    public string SessionId { get; init; }
    public string SecurityKey { get; init; }
}

public record BasicUserModel {
    [JsonPropertyName("id"),
     JsonConverter(typeof(IntToStringConverter))]
    public string Id { get; init; }
    
    [JsonPropertyName("username")]
    public string Username { get; init; }
}

// Shares properties from Rest User/User, PHP User Avi Card, Rest Account
public record MinimalUserModel : BasicUserModel {
    [JsonPropertyName("age"),
     JsonConverter(typeof(NullToIntConverter))]
    public int Age { get; init; }
    
    [JsonPropertyName("gender"),
     JsonConverter(typeof(UserGenderConverter))]
    public string Gender { get; init; }
    
    [JsonPropertyName("is_vip")]
    public bool IsVip { get; init; }
    
    [JsonPropertyName("is_ap")]
    public bool IsAp { get; init; }
    
    [JsonConverter(typeof(AvailabilityConverter))]
    public Availability Availability { get; init; }
    
    [JsonPropertyName("is_ageverified")]
    public bool IsAgeverified { get; init; }
    
    [JsonPropertyName("email")]
    public string Email { get; init; }
    
    [JsonPropertyName("last_password_change")]
    public DateTimeOffset LastPasswordChange { get; init; }
}

// Rest User account
public record UserAccount : MinimalUserModel {
    [JsonPropertyName("show_age")]
    public bool IsAgeVisible { get; init; }
    
    [JsonPropertyName("show_gender")]
    public bool IsGenderVisible { get; init; }
    
    [JsonPropertyName("show_ap")]
    public bool IsApVisible { get; init; }
    
    [JsonPropertyName("show_vip")]
    public bool IsVipVisible { get; init; }
    
    [JsonPropertyName("vip_expires")]
    public string VipExpiration { get; init; }
    
    [JsonPropertyName("show_ageverified")]
    public bool ShowAgeVerification { get; init; }
    
    [JsonPropertyName("show_location")]
    public bool ShowLocation { get; init; }
    
    [JsonPropertyName("location")]
    public string Location { get; init; }
    
    [JsonPropertyName("show_current_chat_room")]
    public bool ShowCurrentChatRoom { get; init; }
}

public record UserModelCommon : BasicUserModel {
    [JsonPropertyName("registered"),
     JsonConverter(typeof(DateTimeConverter))]
    // TODO: Ignore the property in PHP or don't override
    public DateTimeOffset RegisteredOn { get; init; }
    
    [JsonPropertyName("badge_level")]
    public int BadgeLevel { get; init; }
    
    [JsonPropertyName("tagline")]
    public string Tagline { get; init; }
    
    [JsonPropertyName("online")]
    public bool IsOnline { get; init; }
    
    [JsonPropertyName("profileImage")]
    public string ProfileImage { get; init; }
    
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
    public DateTimeOffset CreatedOn { get; init; }
    
    [JsonPropertyName("display_name")]
    public string DisplayName { get; init; }
    
    [JsonPropertyName("is_creator"),
     JsonConverter(typeof(ValueToBoolConverter))]
    public bool IsCreator { get; init; }
    
    [JsonPropertyName("is_adult"),
     JsonConverter(typeof(ValueToBoolConverter))]
    public bool IsAdult { get; init; }
    
    [JsonPropertyName("is_staff"),
     JsonConverter(typeof(ValueToBoolConverter))]
    public bool IsStaff { get; init; }
    
    [JsonPropertyName("is_greeter"),
     JsonConverter(typeof(ValueToBoolConverter))]
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
    
    [JsonPropertyName("is_email_verified")]
    public bool IsEmailVerified { get; init; }
    
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
    [JsonPropertyName("is_buddy"),
     JsonConverter(typeof(ValueToBoolConverter))]
    public bool IsBuddy { get; init; }
    
    [JsonPropertyName("is_friend"),
     JsonConverter(typeof(ValueToBoolConverter))]
    public bool IsFriend { get; init; }
    
    [JsonPropertyName("is_qa"),
     JsonConverter(typeof(ValueToBoolConverter))]
    public bool IsQa { get; init; }
    
    [JsonPropertyName("last_login"),
     JsonConverter(typeof(DateTimeConverter))]
    public DateTimeOffset LastLogin { get; init; }
    
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