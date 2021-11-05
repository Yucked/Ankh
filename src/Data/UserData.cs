using System.Text.Json;
using System.Text.Json.Serialization;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable NotAccessedPositionalProperty.Global

namespace Ankh.Data;

/// <summary>
/// 
/// </summary>
/// <param name="Location"></param>
/// <param name="CountryCode"></param>
/// <param name="State"></param>
/// <param name="IsFlagIconVisible"></param>
/// <param name="IsFlagVisible"></param>
public record struct UserLocation(string Location, int CountryCode, object State,
                                  bool IsFlagIconVisible, bool IsFlagVisible);

/// <summary>
/// 
/// </summary>
/// <param name="Url"></param>
/// <param name="IsDefault"></param>
public record struct Avatar(string Url, bool IsDefault);

/// <summary>
/// 
/// </summary>
/// <param name="IsModerator"></param>
/// <param name="WelcomeScore"></param>
public record struct Moderator(bool IsModerator, int WelcomeScore);

/// <summary>
/// 
/// </summary>
/// <param name="IsBuddy"></param>
/// <param name="IsFriend"></param>
/// <param name="IsQualityAssurance"></param>
/// <param name="ShowMessage"></param>
/// <param name="ShowBlock"></param>
public record struct Misc(bool IsBuddy, bool IsFriend, bool IsCreator,
                          bool IsQualityAssurance, bool ShowMessage, bool ShowBlock);

/// <summary>
/// 
/// </summary>
/// <param name="Status"></param>
/// <param name="Orientation"></param>
/// <param name="LookingFor"></param>
public record struct Dating(string Status, string Orientation, string LookingFor);

/// <summary>
/// 
/// </summary>
public readonly record struct UserData : IData {
    [JsonIgnore]
    public int CId { get; private init; }

    [JsonPropertyName("username")]
    public string Username { get; private init; }

    [JsonPropertyName("homepage")]
    public string Homepage { get; private init; }

    [JsonPropertyName("registered_on")]
    public DateTime RegisteredOn { get; private init; }

    [JsonPropertyName("last_logon")]
    public DateTime LastLogon { get; private init; }

    [JsonPropertyName("interests")]
    public string Interests { get; private init; }

    [JsonPropertyName("gender")]
    public string Gender { get; private init; }

    [JsonPropertyName("age")]
    public int Age { get; private init; }

    [JsonPropertyName("tagline")]
    public string Tagline { get; private init; }

    [JsonPropertyName("online")]
    public bool IsOnline { get; private init; }

    [JsonPropertyName("availability")]
    public string Availability { get; private init; }

    [JsonPropertyName("visible_albums")]
    public int VisibleAlbums { get; private init; }

    [JsonPropertyName("location")]
    public UserLocation Location { get; private init; }

    [JsonPropertyName("avatar")]
    public Avatar Picture { get; private init; }

    [JsonPropertyName("moderator")]
    public Moderator Moderator { get; private init; }

    [JsonPropertyName("misc")]
    public Misc Misc { get; private init; }

    [JsonPropertyName("dating")]
    public Dating Dating { get; private init; }

    [JsonPropertyName("badges")]
    public BadgesData Badges { get; private init; }

    [JsonPropertyName("public_rooms")]
    public IReadOnlyCollection<RoomData> PublicRooms { get; private init; }

    [JsonPropertyName("usernames")]
    public HashSet<string> Usernames { get; private init; }

    [JsonPropertyName("id")]
    public string Id
        => $"{CId}";

    public static async ValueTask<UserData> BuildUserAsync(Stream stream) {
        using var document = await JsonDocument.ParseAsync(stream);
        var rootElement = document.RootElement;

        UserLocation GetLocation() {
            return new UserLocation {
                Location = rootElement.GetProperty("location").GetString(),
                CountryCode = rootElement.GetProperty("country_code").GetInt32(),
                State = rootElement.GetProperty("location_state"),
                IsFlagVisible = rootElement.GetProperty("show_flag_av").GetInt32() == 1,
                IsFlagIconVisible = rootElement.GetProperty("show_flag_icon").GetInt32() == 1
            };
        }

        Avatar GetAvatarPicture() {
            return new Avatar {
                Url = rootElement.GetProperty("avpic_url").GetString(),
                IsDefault = rootElement.GetProperty("avpic_default").GetInt32() == 1
            };
        }

        Moderator GetModeratorData() {
            return new Moderator {
                WelcomeScore = rootElement.GetProperty("welcome_moderator_score").GetInt32(),
                IsModerator = rootElement.GetProperty("is_welcome_moderator").GetInt32() == 1
            };
        }

        Misc GetMiscUserData() {
            return new Misc {
                IsBuddy = rootElement.GetProperty("is_buddy").GetBoolean(),
                IsFriend = rootElement.GetProperty("is_friend").GetInt32() == 1,
                IsQualityAssurance = rootElement.GetProperty("is_qa").GetBoolean(),
                ShowBlock = rootElement.GetProperty("show_block").GetBoolean(),
                ShowMessage = rootElement.GetProperty("show_message").GetInt32() == 1,
                IsCreator = rootElement.TryGetProperty("is_creator", out var isCreator) 
                            && isCreator.GetInt32() == 1
            };
        }

        Dating GetDating() {
            var dating = rootElement.GetProperty("dating");
            return new Dating {
                Status = dating.GetProperty("relationship_status").GetString(),
                Orientation = dating.GetProperty("orientation").GetString(),
                LookingFor = dating.GetProperty("looking_for").GetString()
            };
        }

        return new UserData {
            CId = rootElement.GetProperty("cid").GetInt32(),
            Username = rootElement.GetProperty("avname").GetString(),
            Homepage = rootElement.GetProperty("url").GetString(),
            RegisteredOn = DateTime.Parse(rootElement.GetProperty("registered").GetString()!),
            LastLogon = DateTime.Parse(rootElement.GetProperty("last_login").GetString()!),
            Gender = rootElement.GetProperty("gender").GetString(),
            Tagline = rootElement.GetProperty("tagline").GetString(),
            IsOnline = rootElement.GetProperty("online").GetBoolean(),
            Availability = rootElement.GetProperty("availability").GetString(),
            VisibleAlbums = rootElement.GetProperty("visible_albums").GetInt32(),
            Location = GetLocation(),
            Picture = GetAvatarPicture(),
            Moderator = GetModeratorData(),
            Misc = GetMiscUserData(),
            Dating = GetDating(),
            Badges = BadgesData.GetBadgesData(rootElement),
            Interests = rootElement.GetProperty("interests")
                .GetProperty("full_text_string")
                .GetProperty("tag")
                .GetString(),
            Usernames = new HashSet<string>(),
            PublicRooms = new List<RoomData>(),
            Age = int.TryParse(rootElement.GetProperty("age").GetString(), out var age)
                ? age
                : 0
        };
    }
}