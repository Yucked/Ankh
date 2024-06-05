using System.Text.Json.Serialization;

namespace Ankh.Models.Rest;

public sealed record RestCreatorModel(
    [property: JsonPropertyName("show_tier")]
    bool IsTierVisible,
    [property: JsonPropertyName("creator_tier")]
    int CreatorTier,
    [property: JsonPropertyName("is_pro")]
    bool IsPro,
    [property: JsonPropertyName("is_active")]
    bool IsActive,
    [property: JsonPropertyName("is_top_seller")]
    bool IsTopSeller,
    [property: JsonPropertyName("is_veteran")]
    bool IsVeteran,
    [property: JsonPropertyName("is_media_maven")]
    bool IsMediaMaven,
    [property: JsonPropertyName("is_media_master")]
    bool IsMediaMaster
) : IRestModel;