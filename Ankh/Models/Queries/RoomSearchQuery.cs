using Ankh.Models.Enums;

namespace Ankh.Models.Queries;

public sealed record RoomSearchQuery {
    /// <summary>
    /// 
    /// </summary>
    public string Keywords { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public string Language { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public string Username { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public int? MinOccupants { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public int? MaxOccupants { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public bool? HasPlusProducts { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public bool? RequiresAccessPass { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public ContentRating? Rating { get; set; }
}