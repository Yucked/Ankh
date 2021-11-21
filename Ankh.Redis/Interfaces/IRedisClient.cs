using StackExchange.Redis;

namespace Ankh.Redis.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IRedisClient<T>
    where T : IRedisEntity {
    /// <summary>
    /// 
    /// </summary>
    IDatabaseAsync Database { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="commandFlags"></param>
    /// <returns></returns>
    ValueTask<T> GetAsync(string key,
                          CommandFlags commandFlags = CommandFlags.None);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="commandFlags"></param>
    /// <returns></returns>
    ValueTask<int> CountAsync(CommandFlags commandFlags = CommandFlags.None);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    ValueTask<bool> ExistsAsync(string key);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="commandFlags"></param>
    /// <returns></returns>
    ValueTask<IReadOnlyCollection<T>> GetAllAsync(CommandFlags commandFlags = CommandFlags.None);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="when"></param>
    /// <param name="commandFlags"></param>
    /// <returns></returns>
    ValueTask AddAsync(T value,
                       When when = When.Always,
                       CommandFlags commandFlags = CommandFlags.None);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="commandFlags"></param>
    /// <returns></returns>
    ValueTask RemoveAsync(string key,
                          CommandFlags commandFlags = CommandFlags.None);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="command"></param>
    /// <param name="commandFlags"></param>
    /// <returns></returns>
    ValueTask ExecuteAsync(string command,
                           CommandFlags commandFlags = CommandFlags.None);
}