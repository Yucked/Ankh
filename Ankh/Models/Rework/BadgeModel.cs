using System.Text.Json.Serialization;
using Ankh.Converters;

namespace Ankh.Models.Rework;

public record BadgeModel {
    [JsonPropertyName("badgeid")]
    public string Id { get; init; }
    
    [JsonPropertyName("creator_id"),
     JsonConverter(typeof(IntToStringConverter))]
    public string UserId { get; init; }
    
    [JsonPropertyName("creator_badge_index")]
    public int Index { get; init; }
    
    [JsonPropertyName("name")]
    public string Name { get; init; }
    
    [JsonPropertyName("image_url")]
    public string ImageUrl { get; init; }
    
    [JsonPropertyName("description")]
    public string Description { get; init; }
    
    [JsonPropertyName("badge_type"),
     JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int BadgeType { get; init; }
    
    [JsonPropertyName("allow_autogrant"),
     JsonConverter(typeof(ValueToBoolConverter))]
    public bool IsAutoGrantAllowed { get; init; }
    
    [JsonPropertyName("review_status")]
    public string ReviewStatus { get; init; }
    
    [JsonPropertyName("image_height")]
    public int Height { get; init; }
    
    [JsonPropertyName("image_width")]
    public int Width { get; init; }
    
    [JsonPropertyName("flagger_id")]
    public string FlaggerId { get; init; }
    
    [JsonPropertyName("flag_time"),
     JsonConverter(typeof(DateTimeConverter))]
    public DateTimeOffset FlaggedOn { get; init; }
}

public record BadgeModelProfile : BadgeModel {
    [JsonPropertyName("xloc")]
    public int Xloc { get; init; }
    
    [JsonPropertyName("yloc")]
    public int Yloc { get; init; }
}