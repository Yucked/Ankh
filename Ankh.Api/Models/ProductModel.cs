using System.Text.Json.Serialization;
using Ankh.Api.Converters;
using Ankh.Api.Models.Enums;

// ReSharper disable All

namespace Ankh.Api.Models;

public record ProductModel(
    [property: JsonPropertyName("product_id")]
    long ProductId,
    [property: JsonPropertyName("product_name")]
    string ProductName,
    [property: JsonPropertyName("creator_cid")]
    long CreatorId,
    [property: JsonPropertyName("creator_name")]
    string CreatorName,
    [property: JsonPropertyName("rating"),
               JsonConverter(typeof(ContentRatingConverter))]
    ContentRating Rating,
    [property: JsonPropertyName("product_price")]
    long Price,
    [property: JsonPropertyName("discount_price")]
    long DiscountPrice,
    [property: JsonPropertyName("product_page")]
    string Url,
    [property: JsonPropertyName("creator_page")]
    Uri CreatorPage,
    [property: JsonPropertyName("is_bundle")]
    bool IsBundle,
    [property: JsonPropertyName("profit")]
    long Profit,
    [property: JsonPropertyName("derivation_profit")]
    long DerivationProfit,
    [property: JsonPropertyName("allows_derivation"),
               JsonConverter(typeof(IntToBoolConverter))]
    bool AllowsDerivation,
    [property: JsonPropertyName("allow_third_party_bundle"),
               JsonConverter(typeof(IntToBoolConverter))]
    bool AllowThirdPartyBundle,
    [property: JsonPropertyName("product_image")]
    string Image,
    [property: JsonPropertyName("category_path")]
    CategoryPath[] CategoryPath,
    [property: JsonPropertyName("gender")]
    string Gender,
    [property: JsonPropertyName("categories")]
    string[] Categories,
    [property: JsonPropertyName("look_url")]
    object LookUrl,
    [property: JsonPropertyName("is")]
    string[] Is,
    [property: JsonPropertyName("compatible_body_patterns")]
    long[] CompatibleBodyPatterns,
    [property: JsonPropertyName("has_plus_badge")]
    bool HasPlusBadge,
    [property: JsonPropertyName("is_plus_badge_compatible")]
    bool IsPlusBadgeCompatible,
    [property: JsonPropertyName("gender_restriction")]
    string GenderRestriction,
    [property: JsonPropertyName("is_bundable"),
               JsonConverter(typeof(IntToBoolConverter))]
    bool IsBundable,
    [property: JsonPropertyName("is_visible")]
    bool IsVisible,
    [property: JsonPropertyName("is_wearable_in_pure")]
    bool IsWearableInPure,
    [property: JsonPropertyName("allow_delete")]
    bool AllowDelete,
    [property: JsonPropertyName("is_nft")]
    bool IsNFT,
    [property: JsonPropertyName("is_purchasable")]
    bool IsPurchasable,
    [property: JsonPropertyName("supports_youtube")]
    bool SupportsYoutube,
    [property: JsonPropertyName("default_orientation")]
    string DefaultOrientation,
    [property: JsonPropertyName("tags")]
    string[] Tags,
    [property: JsonPropertyName("preview_image_supports_no_redirect")]
    bool PreviewImageSupportsNoRedirect,
    [property: JsonPropertyName("preview_image")]
    Uri PreviewImage,
    [property: JsonPropertyName("allow_derived_nft_minting"),
               JsonConverter(typeof(IntToBoolConverter))]
    bool AllowDerivedNftMinting
) : IRestModel;

public record CategoryPath(
    [property: JsonPropertyName("id")]
    long Id,
    [property: JsonPropertyName("name")]
    string Name);