namespace Ankh.Data;

public sealed record DirectoryData {
    /// <summary>
    /// 
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public HashSet<string> Records { get; init; }

    public static DirectoryData Update(DirectoryData before, DirectoryData after) {
        before.Records.UnionWith(after.Records);
        return before;
    }
}