using System.Globalization;
using System.Text.Json;

namespace Ankh.Data;

public sealed class Badge {
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsAutogranted { get; set; }
    public string Type { get; set; }
    public string ReviewStatus { get; set; }
    public string Url { get; set; }
    public Creator Creator { get; set; }
    public Flag Flag { get; set; }
    public Dimensions Dimensions { get; set; }
    public Coordinates Coordinates { get; set; }
}


public sealed class Dimensions {
    public int Width { get; init; }
    public int Height { get; init; }
}


public sealed class Flag {
    public string Id { get; init; }
    public DateTime FlaggedOn { get; init; }
}

public sealed class Coordinates {
    public int X { get; init; }
    public int Y { get; init; }
}


public sealed class Creator {
    public string Id { get; init; }
    public int Index { get; init; }
}

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