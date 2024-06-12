using Ankh.Models.Enums;

namespace Ankh.Models.Queries;

/// <summary>
/// https://api.imvu.com/product?
/// product_rating=0
/// filter_text=wig
/// name_filter=wig
/// partial_avatar_name=wig
/// keywords=wig
/// cat=40%2C41
/// gender_restriction=male_compatible
/// no_cat=316%2C324
/// vcoin_price_min=0
/// price_min=1
/// include_histogram=true
/// </summary>
public sealed record ProductSearchQuery {
    /// <summary>
    /// 
    /// </summary>
    public ContentRating ProductRating { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public string FilterText { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public string FilterName { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public string PartialAvatarName { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public string Keywords { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public int[]? IncludedCategories { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public char? Gender { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public char? GenderRestriction { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public int[]? ExlcudedCategories { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public int MinimumVCoinPrice { get; set; } = 1;
    
    /// <summary>
    /// 
    /// </summary>
    public int MinimumPrice { get; set; } = 1;
    
    /// <summary>
    /// 
    /// </summary>
    public bool IncludeHistogram { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public int Limit { get; set; } = 10;
}