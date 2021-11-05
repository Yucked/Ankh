using System.Text.Json.Serialization;

namespace Ankh.Data;

public record struct Badge {
    [JsonPropertyName("creator_id")]
    public int CreatorId { get; init; }

    [JsonPropertyName("creator_badge_index")]
    public int CreatorIndex { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; }

    [JsonPropertyName("image_mogilekey")]
    public string ImageMogilekey { get; init; }

    [JsonPropertyName("image_width")]
    public int Width { get; init; }

    [JsonPropertyName("image_height")]
    public int Height { get; init; }

    [JsonPropertyName("description")]
    public string Description { get; init; }

    [JsonPropertyName("allow_autogrant")]
    public string AllowAutogrant { get; init; }

    [JsonPropertyName("badge_type")]
    public string BadgeType { get; init; }

    [JsonPropertyName("review_status")]
    public string ReviewStatus { get; init; }

    [JsonPropertyName("flagger_id")]
    public string FlaggerId { get; init; }

    [JsonPropertyName("flag_time")]
    public string FlagTime { get; init; }

    [JsonPropertyName("badgeid")]
    public string Badgeid { get; init; }

    [JsonPropertyName("image_url")]
    public string ImageUrl { get; init; }

    [JsonPropertyName("xloc")]
    public int Xloc { get; init; }

    [JsonPropertyName("yloc")]
    public int Yloc { get; init; }
}

public record struct BadgesData {
    [JsonPropertyName("badge_count")]
    public int Count { get; private init; }

    [JsonPropertyName("badge_level")]
    public int Level { get; private init; }

    [JsonPropertyName("badge_area_html")]
    public string HtmlLayout { get; private init; }

    [JsonPropertyName("show_badgecount")]
    public bool IsCountVisible { get; private init; }
}