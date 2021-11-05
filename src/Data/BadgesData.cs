using System.Text.Json;

namespace Ankh.Data;

/// <summary>
/// 
/// </summary>
/// <param name="Count"></param>
/// <param name="Level"></param>
/// <param name="Layout"></param>
/// <param name="IsCountVisible"></param>
/// <param name="Badges"></param>
public record struct BadgesData(int Count, int Level, string Layout,
                                bool IsCountVisible, IEnumerable<Badge> Badges) {
    internal static BadgesData GetBadgesData(JsonElement jsonElement) {
        return new BadgesData {
            Count = jsonElement.GetProperty("badge_count").GetInt32(),
            Level = jsonElement.GetProperty("badge_level").GetInt32(),
            Layout = jsonElement.GetProperty("badge_area_html").GetString(),
            IsCountVisible = jsonElement.GetProperty("show_badgecount").GetBoolean(),
            Badges = GetBadges(jsonElement.GetProperty("badge_layout"))
        };
    }

    private static IEnumerable<Badge> GetBadges(JsonElement jsonElement) {
        return jsonElement.EnumerateObject()
            .Select(property => property.Value)
            .Select(badge => new Badge {
                Id = badge.GetProperty("badgeid").GetString(),
                Name = badge.GetProperty("name").GetString(),
                IsAutogranted = badge.GetProperty("allow_autogrant").GetInt32() == 1,
                Type = badge.GetProperty("badge_type").GetString(),
                ReviewStatus = badge.GetProperty("review_status").GetString(),
                Url = badge.GetProperty("image_url").GetString(),
                Creator = new Creator {
                    Id = $"{badge.GetProperty("creator_id").GetInt32()}",
                    Index = badge.GetProperty("creator_badge_index").GetInt32()
                },
                Flag = new Flag {
                    Id = badge.GetProperty("flagger_id").GetString(),
                    Time = DateTime.Parse(badge.GetProperty("flag_time").GetString()!)
                },
                Coordinates = new Coordinates {
                    X = badge.GetProperty("xloc").GetInt32(),
                    Y = badge.GetProperty("yloc").GetInt32()
                },
                Dimensions = new Dimensions {
                    Width = badge.GetProperty("image_width").GetInt32(),
                    Height = badge.GetProperty("image_height").GetInt32(),
                }
            });
    }
}

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="IsAutogranted"></param>
/// <param name="Type"></param>
/// <param name="ReviewStatus"></param>
/// <param name="Url"></param>
/// <param name="Creator"></param>
/// <param name="Flag"></param>
/// <param name="Dimensions"></param>
/// <param name="Coordinates"></param>
public record struct Badge(string Id, string Name, bool IsAutogranted,
                           string Type, string ReviewStatus, string Url,
                           Creator Creator, Flag Flag,
                           Dimensions Dimensions, Coordinates Coordinates);

/// <summary>
/// 
/// </summary>
/// <param name="Width"></param>
/// <param name="Height"></param>
public record struct Dimensions(int Width, int Height);

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Time"></param>
public record struct Flag(string Id, DateTime Time);

/// <summary>
/// 
/// </summary>
/// <param name="X"></param>
/// <param name="Y"></param>
public record struct Coordinates(int X, int Y);

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Index"></param>
public record struct Creator(string Id, int Index);