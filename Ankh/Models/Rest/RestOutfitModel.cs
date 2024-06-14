using System.Text.Json.Serialization;
using Ankh.Converters;
using Ankh.Models.Enums;

namespace Ankh.Models.Rest;

public sealed record RestOutfitModel(
    [property: JsonPropertyName("outfit_id")]
    int OutfitId,
    [property: JsonPropertyName("list_id")]
    int ListId,
    [property: JsonPropertyName("outfit_name")]
    string Name,
    [property: JsonPropertyName("dirty"),
               JsonConverter(typeof(IntToBoolConverter))]
    bool IsDirty,
    [property: JsonPropertyName("full_image")]
    string FullImage,
    [property: JsonPropertyName("outfit_image")]
    string OutfitImage,
    [property: JsonPropertyName("is_outfit_image_redirect")]
    bool IsOutfitImageRedirect,
    [property: JsonPropertyName("created")]
    DateTimeOffset Created,
    [property: JsonPropertyName("last_updated")]
    DateTimeOffset LastUpdated,
    [property: JsonPropertyName("look_url")]
    string LookUrl,
    [property: JsonPropertyName("rating"),
               JsonConverter(typeof(ContentRatingConverter))]
    ContentRating Rating,
    [property: JsonPropertyName("pids")]
    int[] ProductIds,
    [property: JsonPropertyName("privacy")]
    string Privacy // TODO: Figure out Privacy
) ;