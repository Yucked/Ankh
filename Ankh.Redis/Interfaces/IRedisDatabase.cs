using StackExchange.Redis;

namespace Ankh.Redis.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IRedisDatabase {
    /// <summary>
    /// 
    /// </summary>
    IDatabaseAsync Database { get; }

    string Prefix
        t
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    ValueTask<T> GetAsync<T>(string key);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="commandFlags"></param>
    /// <returns></returns>
    ValueTask<T> GetAsync<T>(string key, CommandFlags commandFlags = CommandFlags.None);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    ValueTask<bool> ExistsAsync<T>(string key);
}
