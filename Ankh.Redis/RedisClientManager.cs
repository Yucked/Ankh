using Ankh.Redis.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Ankh.Redis;

/// <summary>
/// 
/// </summary>
public class RedisClientManager {
    private readonly RedisClientConnection _clientConnection;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    public RedisClientManager(IConfiguration configuration) {
        _clientConnection = configuration
            .GetSection("Redis")
            .Get<RedisClientConnection>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IRedisClient<T> For<T>(RedisClientOptions clientOptions = default)
        where T : IRedisEntity {
        var options = clientOptions.Equals(default)
            ? new RedisClientOptions {
                DatabaseIndex = 1,
                TypeNameAsKeyPrefix = true
            }
            : clientOptions;

        return new RedisClient<T>(
            ConnectionMultiplexer.Connect(_clientConnection.Connection),
            options);
    }
}