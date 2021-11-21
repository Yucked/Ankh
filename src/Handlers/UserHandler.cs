using System.Globalization;
using System.Text.Json;
using Ankh.Data;
using Ankh.Redis;

namespace Ankh.Handlers;

public sealed class UserHandler {
    private readonly RedisClientManager _redisClientManager;

    public UserHandler(RedisClientManager redisClientManager) {
        _redisClientManager = redisClientManager;
    }

    public async Task JsonToUserAsync(Stream stream) {
        using var document = await JsonDocument.ParseAsync(stream);
        var rootElement = document.RootElement;

        UserLocation GetLocation() {
            return new UserLocation {
                Country = rootElement.Get<string>("location"),
                CountryCode = rootElement.Get<int>("country_code"),
                State = rootElement.Get<string>("location_state"),
                IsFlagVisible = rootElement.Get<bool>("show_flag_av", JsonGetOptions.IntToBool),
                IsFlagIconVisible = rootElement.Get<bool>("show_flag_icon", JsonGetOptions.IntToBool)
            };
        }

        Avatar GetAvatarPicture() {
            return new Avatar {
                Url = rootElement.Get<string>("avpic_url"),
                IsDefault = rootElement.Get<bool>("avpic_default", JsonGetOptions.IntToBool)
            };
        }

        Moderator GetModeratorData() {
            return new Moderator {
                WelcomeScore = rootElement.Get<int>("welcome_moderator_score"),
                IsModerator = rootElement.Get<bool>("is_welcome_moderator", JsonGetOptions.IntToBool)
            };
        }

        Misc GetMiscUserData() {
            return new Misc {
                IsBuddy = rootElement.Get<bool>("is_buddy"),
                IsFriend = rootElement.Get<bool>("is_friend", JsonGetOptions.IntToBool),
                IsQualityAssurance = rootElement.Get<bool>("is_qa"),
                ShowBlock = rootElement.Get<bool>("show_block"),
                ShowMessage = rootElement.Get<bool>("show_message", JsonGetOptions.IntToBool),
                IsCreator = rootElement.Get<bool>("is_creator", JsonGetOptions.IntToBool),
                ImvuLevel = rootElement.Get<int>("imvu_level"),
                WallpaperId = rootElement.Get<int>("wallpaper_id"),
            };
        }

        Dating GetDating() {
            var dating = rootElement.GetProperty("dating");
            return new Dating {
                Status = dating.Get<string>("relationship_status"),
                Orientation = dating.Get<string>("orientation"),
                LookingFor = dating.Get<string>("looking_for")
            };
        }

        int GetAge() {
            return int.TryParse($"{rootElement.GetProperty("age")}", out var age)
                ? age
                : default;
        }

        string GetInterest() {
            return rootElement.GetProperty("interests")
                .GetProperty("full_text_string")
                .Get<string>("tag");
        }

        var userData = new UserData {
            Id = rootElement.GetProperty("cid").GetInt32().ToString(),
            Username = rootElement.Get<string>("avname"),
            Homepage = rootElement.Get<string>("url"),
            RegisteredOn = rootElement.Get<DateOnly>("registered", JsonGetOptions.ParseDate),
            LastLogon = rootElement.Get<DateOnly>("last_login", JsonGetOptions.ParseDate),
            Gender = rootElement.Get<string>("gender"),
            Tagline = rootElement.Get<string>("tagline"),
            IsOnline = rootElement.Get<bool>("online"),
            Availability = rootElement.Get<string>("availability"),
            VisibleAlbums = rootElement.Get<int>("visible_albums"),
            Location = GetLocation(),
            Avatar = GetAvatarPicture(),
            Moderator = GetModeratorData(),
            Misc = GetMiscUserData(),
            Dating = GetDating(),
            Interests = GetInterest(),
            Age = GetAge(),
            Usernames = new HashSet<string>(),
            AddedOn = DateOnly.FromDateTime(DateTime.Now)
        };

        var badges = new Badges {
            Count = rootElement.Get<int>("badge_count"),
            Level = rootElement.Get<int>("badge_level"),
            Layout = rootElement.Get<string>("badge_area_html"),
            IsCountVisible = rootElement.Get<bool>("show_badgecount"),
        };

        if (rootElement.ValueKind == JsonValueKind.Array &&
            rootElement.GetArrayLength() != 0) {
            var badgeDatas = GetBadgesData(rootElement).ToArray();
            badges.BadgesId = badgeDatas.Select(x => x.Id);

            var badgeClient = _redisClientManager.For<BadgeData>();
            await Parallel.ForEachAsync(badgeDatas, async (data, _) => {
                if (await badgeClient.ExistsAsync(data.Id)) {
                    //await badgeClient.AddAsync(data, )
                    return;
                }

                await badgeClient.AddAsync(data);
            });
        }

        userData.Badges = badges;
        await _redisClientManager.For<UserData>().AddAsync(userData);
    }

    private static IEnumerable<BadgeData> GetBadgesData(JsonElement jsonElement) {
        return jsonElement.EnumerateObject()
            .Select(x => new BadgeData {
                Id = x.Get<string>("badgeid"),
                Name = x.Get<string>("name"),
                IsAutogranted = x.Get<bool>("allow_autogrant", JsonGetOptions.IntToBool),
                Type = x.Get<string>("badge_type"),
                ReviewStatus = x.Get<string>("review_status"),
                Url = x.Get<string>("image_url"),
                Creator = new Creator {
                    Id = x.Get<string>("creator_id", JsonGetOptions.IntToString),
                    Index = x.Get<int>("creator_badge_index")
                },
                Flag = new Flag {
                    Id = x.Get<string>("flagger_id"),
                    FlaggedOn = GetDate(x.Value)
                },
                Coordinates = new Coordinates {
                    X = x.Get<int>("xloc"),
                    Y = x.Get<int>("yloc")
                },
                Dimensions = new Dimensions {
                    Width = x.Get<int>("image_width"),
                    Height = x.Get<int>("image_height")
                }
            });
    }

    private static DateTime GetDate(JsonElement jsonElement) {
        var dateString = jsonElement.Get<string>("flag_time");
        return dateString
            .Replace(" ", string.Empty)
            .Replace(":", string.Empty)
            .Replace("-", string.Empty)
            .All(x => x == '0')
            ? DateTime.MinValue
            : DateTime.ParseExact(dateString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
    }

    public static UserData Update(UserData before, UserData after) {
        var updated = after.Update(before);
        if (!updated.Usernames.Contains(before.Username)) {
            updated.Usernames.Add(before.Username);
        }

        if (!updated.Usernames.Contains(after.Username)) {
            updated.Usernames.Add(after.Username);
        }

        return updated;
    }
}