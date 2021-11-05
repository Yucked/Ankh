namespace Ankh.Data;

public record struct RoomData : IData {
    /// <summary>
    /// 
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string Owner { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public int Capacity { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public int Occupancy { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string Image { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsApOnly { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsVipOnly { get; init; }

    /// <summary>
    /// ???
    /// </summary>
    public bool ApNameOnly { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public int Ratings { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string Url { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public IDictionary<string, DateTimeOffset> UserHistory { get; init; }
}