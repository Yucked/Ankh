using System.Globalization;
using System.Text.Json;

namespace Ankh.Data;

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
/// <param name="FlaggedOn"></param>
public record struct Flag(string Id, DateTime FlaggedOn);

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

public sealed class BadgesData {
    public int Count { get; set; }
    public int Level { get; set; }
    public string Layout { get; set; }
    public bool IsCountVisible { get; set; }
    public IEnumerable<Badge> Badges { get; set; }

    internal static BadgesData GetBadgesData(JsonElement jsonElement) {
        return new BadgesData {
            Count = jsonElement.GetProperty("badge_count").GetInt32(),
            Level = jsonElement.GetProperty("badge_level").GetInt32(),
            Layout = jsonElement.GetProperty("badge_area_html").GetString(),
            IsCountVisible = jsonElement.GetProperty("show_badgecount").GetBoolean(),
            Badges = GetBadges(jsonElement.GetProperty("badge_layout"))
        };
    }

    private static IReadOnlyList<Badge> GetBadges(JsonElement jsonElement) {
        static DateTime GetDate(JsonElement jsonElement) {
            var dateString = jsonElement.GetProperty("flag_time").GetString()!;
            return dateString
                .Replace(" ", string.Empty)
                .Replace(":", string.Empty)
                .Replace("-", string.Empty)
                .All(x => x == '0')
                ? DateTime.MinValue
                : DateTime.ParseExact(dateString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        if (jsonElement.ValueKind == JsonValueKind.Array && jsonElement.GetArrayLength() == 0) {
            return default;
        }

        return jsonElement.EnumerateObject()
            .Select(x => new Badge {
                Id = x.Value.GetProperty("badgeid").GetString(),
                Name = x.Value.GetProperty("name").GetString(),
                IsAutogranted = int.Parse(x.Value.GetProperty("allow_autogrant").GetString()) == 1,
                Type = x.Value.GetProperty("badge_type").GetString(),
                ReviewStatus = x.Value.GetProperty("review_status").GetString(),
                Url = x.Value.GetProperty("image_url").GetString(),
                Creator = new Creator {
                    Id = $"{x.Value.GetProperty("creator_id").GetInt32()}",
                    Index = x.Value.GetProperty("creator_badge_index").GetInt32()
                },
                Flag = new Flag {
                    Id = x.Value.GetProperty("flagger_id").GetString(),
                    FlaggedOn = GetDate(x.Value)
                },
                Coordinates = new Coordinates {
                    X = x.Value.GetProperty("xloc").GetInt32(),
                    Y = x.Value.GetProperty("yloc").GetInt32()
                },
                Dimensions = new Dimensions {
                    Width = x.Value.GetProperty("image_width").GetInt32(),
                    Height = x.Value.GetProperty("image_height").GetInt32(),
                }
            })
            .ToArray();
    }
}