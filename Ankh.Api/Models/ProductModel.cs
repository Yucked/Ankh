using System.Text.Json.Serialization;

// ReSharper disable All

namespace Ankh.Api.Models;

public class ProductModel : RestResponse {
    [JsonPropertyName("product_id")]
    public long ProductId { get; set; }
    
    [JsonPropertyName("product_name")]
    public string ProductName { get; set; }
    
    [JsonPropertyName("creator_cid")]
    public long CreatorId { get; set; }
    
    [JsonPropertyName("creator_name")]
    public string CreatorName { get; set; }
    
    // TODO: Use ENUM attribute
    [JsonPropertyName("rating")]
    public ProductRating Rating { get; set; }
    
    [JsonPropertyName("product_price")]
    public long Price { get; set; }
    
    [JsonPropertyName("discount_price")]
    public long DiscountPrice { get; set; }
    
    [JsonPropertyName("product_page")]
    public string Url { get; set; }
    
    [JsonPropertyName("creator_page")]
    public Uri CreatorPage { get; set; }
    
    [JsonPropertyName("is_bundle")]
    public bool IsBundle { get; set; }
    
    [JsonPropertyName("profit")]
    public long Profit { get; set; }
    
    [JsonPropertyName("derivation_profit")]
    public long DerivationProfit { get; set; }
    
    [JsonPropertyName("allows_derivation")]
    public long AllowsDerivation { get; set; }
    
    [JsonPropertyName("allow_third_party_bundle")]
    public long AllowThirdPartyBundle { get; set; }
    
    [JsonPropertyName("product_image")]
    public string Image { get; set; }
    
    [JsonPropertyName("category_path")]
    public CategoryPath[] CategoryPath { get; set; }
    
    [JsonPropertyName("gender")]
    public string Gender { get; set; }
    
    [JsonPropertyName("categories")]
    public string[] Categories { get; set; }
    
    [JsonPropertyName("look_url")]
    public object LookUrl { get; set; }
    
    [JsonPropertyName("is")]
    public string[] Is { get; set; }
    
    [JsonPropertyName("compatible_body_patterns")]
    public long[] CompatibleBodyPatterns { get; set; }
    
    [JsonPropertyName("has_plus_badge")]
    public bool HasPlusBadge { get; set; }
    
    [JsonPropertyName("is_plus_badge_compatible")]
    public bool IsPlusBadgeCompatible { get; set; }
    
    [JsonPropertyName("gender_restriction")]
    public string GenderRestriction { get; set; }
    
    [JsonPropertyName("is_bundable")]
    public long IsBundable { get; set; }
    
    [JsonPropertyName("is_visible")]
    public bool IsVisible { get; set; }
    
    [JsonPropertyName("is_wearable_in_pure")]
    public bool IsWearableInPure { get; set; }
    
    [JsonPropertyName("allow_delete")]
    public bool AllowDelete { get; set; }
    
    [JsonPropertyName("is_nft")]
    public bool IsNFT { get; set; }
    
    [JsonPropertyName("is_purchasable")]
    public bool IsPurchasable { get; set; }
    
    [JsonPropertyName("supports_youtube")]
    public bool SupportsYoutube { get; set; }
    
    [JsonPropertyName("default_orientation")]
    public string DefaultOrientation { get; set; }
    
    [JsonPropertyName("tags")]
    public string[] Tags { get; set; }
    
    [JsonPropertyName("preview_image_supports_no_redirect")]
    public bool PreviewImageSupportsNoRedirect { get; set; }
    
    [JsonPropertyName("preview_image")]
    public Uri PreviewImage { get; set; }
    
    [JsonPropertyName("allow_derived_nft_minting")]
    public long AllowDerivedNftMinting { get; set; }
}

public record CategoryPath(
    [property: JsonPropertyName("id")]
    long Id,
    [property: JsonPropertyName("name")]
    string Name);