using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable NotAccessedPositionalProperty.Global

namespace Ankh.Data;

public sealed class UserLocation {
    public string Location { get; init; }
    public int CountryCode { get; init; }
    public string State { get; init; }
    public bool IsFlagIconVisible { get; init; }
    public bool IsFlagVisible { get; init; }
}

public sealed class Avatar {
    public string Url { get; init; }
    public bool IsDefault { get; init; }
}

public sealed class Moderator {
    public bool IsModerator { get; init; }
    public int WelcomeScore { get; init; }
}

public sealed class Misc {
    public bool IsBuddy { get; init; }
    public bool IsFriend { get; init; }
    public bool IsCreator { get; init; }
    public bool IsQualityAssurance { get; init; }
    public bool ShowMessage { get; init; }
    public bool ShowBlock { get; init; }
    public int IMVULevel { get; init; }
    public int WallpaperId { get; init; }
    public bool IsAgeVerified { get; init; }
}

public sealed class Dating {
    public string Status { get; init; }
    public string Orientation { get; init; }
    public string LookingFor { get; init; }
}

public sealed class UserData : IData {
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
    public BadgesData BadgesData { get; private init; }

    [JsonPropertyName("public_rooms")]
    public IReadOnlyCollection<RoomData> PublicRooms { get; private init; }

    [JsonPropertyName("usernames")]
    public HashSet<string> Usernames { get; private init; }

    [JsonPropertyName("id"), Key]
    public string Id
        => $"{CId}";

    public static async ValueTask<UserData> BuildUserAsync(Stream stream) {
        var document = await JsonDocument.ParseAsync(stream);
        var rootElement = document.RootElement;

        UserLocation GetLocation() {
            return new UserLocation {
                Location = rootElement.GetProperty("location").GetString(),
                CountryCode = rootElement.GetProperty("country_code").GetInt32(),
                State = $"{rootElement.GetProperty("location_state")}",
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
                            && isCreator.GetInt32() == 1,
                //IMVULevel = rootElement.GetProperty("imvu_level").GetInt32(),
                //WallpaperId = rootElement.GetProperty("wallpaper_id").GetInt32(),
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
            BadgesData = BadgesData.GetBadgesData(rootElement),
            Interests = rootElement.GetProperty("interests")
                .GetProperty("full_text_string")
                .GetProperty("tag")
                .GetString(),
            Usernames = new HashSet<string>(),
            PublicRooms = new List<RoomData>(),
            Age = int.TryParse($"{rootElement.GetProperty("age")}", out var age)
                ? age
                : 0
        };
    }
}