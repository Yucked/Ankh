using System.Text.Json;
using Ankh.Redis.Interfaces;
using StackExchange.Redis;
using StackExchange.Redis.KeyspaceIsolation;

namespace Ankh.Redis;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed record RedisClient<T>
    : IRedisClient<T> where T : IRedisEntity {
    /// <inheritdoc />
    public IDatabaseAsync Database { get; }

    private readonly RedisClientOptions _clientOptions;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionMultiplexer"></param>
    /// <param name="clientOptions"></param>
    public RedisClient(IConnectionMultiplexer connectionMultiplexer,
                       RedisClientOptions clientOptions) {
        _connectionMultiplexer = connectionMultiplexer;
        _clientOptions = clientOptions;

        Database = _clientOptions.TypeNameAsKeyPrefix
            ? connectionMultiplexer
                .GetDatabase(_clientOptions.DatabaseIndex)
                .WithKeyPrefix(nameof(T))
            : connectionMultiplexer
                .GetDatabase(_clientOptions.DatabaseIndex);
    }

    /// <inheritdoc />
    public async ValueTask<T> GetAsync(string key,
                                       CommandFlags commandFlags = CommandFlags.None) {
        var redisValue = await Database.StringGetAsync(key, commandFlags);
        return JsonSerializer.Deserialize<T>(redisValue, _clientOptions.JsonOptions)!;
    }

    /// <inheritdoc />
    public ValueTask<int> CountAsync(CommandFlags commandFlags = CommandFlags.None) {
        var server = _connectionMultiplexer.GetServer(_clientOptions.Hostname);
        var keys = server.Keys(_clientOptions.DatabaseIndex, $"*{nameof(T)}*");
        return ValueTask.FromResult(keys.Count());
    }

    /// <inheritdoc />
    public async ValueTask<bool> ExistsAsync(string key) {
        return await Database.KeyExistsAsync(key);
    }

    /// <inheritdoc />
    public async ValueTask<IReadOnlyCollection<T>> GetAllAsync(CommandFlags commandFlags = CommandFlags.None) {
        var server = _connectionMultiplexer.GetServer(_clientOptions.Hostname);
        var keys = server.Keys(_clientOptions.DatabaseIndex, $"*{nameof(T)}*");

        var values = new HashSet<T>();
        await Parallel.ForEachAsync(keys, async (key, _) => {
            var value = await GetAsync(key, commandFlags);
            values.Add(value);
        });

        return values;
    }

    /// <inheritdoc />
    public async ValueTask AddAsync(T value,
                                    When when = When.NotExists,
                                    CommandFlags commandFlags = CommandFlags.None) {
        var serialized = JsonSerializer.SerializeToUtf8Bytes(value, _clientOptions.JsonOptions);
        await Database.StringSetAsync(value.Id, serialized);
    }

    /// <inheritdoc />
    public async ValueTask RemoveAsync(string key,
                                       CommandFlags commandFlags = CommandFlags.None) {
        await Database.KeyDeleteAsync(key, commandFlags);
    }

    /// <inheritdoc />
    public async ValueTask ExecuteAsync(string command,
                                        CommandFlags commandFlags = CommandFlags.None) {
        var redisResult = await Database.ExecuteAsync(command);
    }
}