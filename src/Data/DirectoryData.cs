namespace Ankh.Data;

public sealed record DirectoryData : IData {
    /// <summary>
    /// 
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public HashSet<string> Records { get; init; }
}