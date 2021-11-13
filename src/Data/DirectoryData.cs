namespace Ankh.Data;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Records"></param>
public record struct DirectoryData(string Id, HashSet<string> Records);