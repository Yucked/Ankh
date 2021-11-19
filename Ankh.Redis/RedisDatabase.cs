using Ankh.Redis.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Ankh.Redis;
/// <summary>
/// 
/// </summary>
public sealed class RedisDatabase : IRedisDatabase {
    public IDatabaseAsync Database
        => throw new NotImplementedException();

    public ValueTask<bool> ExistsAsync<T>(string key) {
        throw new NotImplementedException();
    }

    public ValueTask<T> GetAsync<T>(string key) {
        throw new NotImplementedException();
    }

    public ValueTask<T> GetAsync<T>(string key, CommandFlags commandFlags = CommandFlags.None) {
        throw new NotImplementedException();
    }
}