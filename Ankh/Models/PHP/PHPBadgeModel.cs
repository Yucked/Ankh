using System.Text.Json.Serialization;
using Ankh.Converters;

namespace Ankh.Models.PHP;

public record PhpBadgeModel(
    [property: JsonPropertyName("creator_id")]
    long CreatorId,
    [property: JsonPropertyName("creator_badge_index")]
    int CreatorBadgeIndex,
    [property: JsonPropertyName("name")]
    string Name,
    [property: JsonPropertyName("image_mogilekey")]
    string ImageMogilekey,
    [property: JsonPropertyName("image_width")]
    int ImageWidth,
    [property: JsonPropertyName("image_height")]
    int ImageHeight,
    [property: JsonPropertyName("description")]
    string Description,
    [property: JsonPropertyName("allow_autogrant"),
               JsonConverter(typeof(ValueToBoolConverter))]
    bool AllowAutogrant,
    [property: JsonPropertyName("badge_type"),
               JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    int BadgeType,
    [property: JsonPropertyName("review_status")]
    string ReviewStatus,
    [property: JsonPropertyName("flagger_id"),
               JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    long FlaggerId,
    [property: JsonPropertyName("flag_time")]
    string FlagTime,
    [property: JsonPropertyName("badgeid")]
    string BadgeId,
    [property: JsonPropertyName("image_url")]
    Uri ImageUrl,
    [property: JsonPropertyName("xloc")]
    int Xloc,
    [property: JsonPropertyName("yloc")]
    int Yloc
);