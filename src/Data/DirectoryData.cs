using Ankh.Redis.Interfaces;

namespace Ankh.Data;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="AddedOn"></param>
/// <param name="Records"></param>
/// <param name="Count"></param>
public record struct DirectoryData(string Id, DateOnly AddedOn,
                                   HashSet<string> Records, int Count) : IRedisEntity;