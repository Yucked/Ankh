namespace Ankh.Redis.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IRedisEntity {
    /// <summary>
    /// 
    /// </summary>
    string Id { get; }

    /// <summary>
    /// 
    /// </summary>
    DateOnly AddedOn { get; }
}