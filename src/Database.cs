using Ankh.Data;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ankh;

public sealed class Database {
    public readonly struct Connection {
        public string EndPoint { get; init; }
        public int Database { get; init; }
        public string Password { get; init; }
        public string ConnectionString
            => $"{EndPoint},defaultDatabase={Database},password={Password}";
    }

    private readonly Connection _connection;
    private readonly ConnectionMultiplexer _connectionMultiplexer;
    private readonly JsonSerializerOptions _serializerOptions;
    public Database(ConnectionMultiplexer connectionMultiplexer, IConfiguration configuration) {
        _connectionMultiplexer = connectionMultiplexer;
        _connection = configuration.GetSection("Connection").Get<Connection>();
        _serializerOptions = new JsonSerializerOptions {
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.Never,
            WriteIndented = true
        };
    }

    public ValueTask<bool> IsConnectAsync() {
        if (!_connectionMultiplexer.IsConnected) {
            return ValueTask.FromResult(false);
        }

        var db = _connectionMultiplexer.GetDatabase();
        return ValueTask.FromResult(db.IsConnected(default));
    }

    public async ValueTask<T> GetAsync<T>(string id) {
        var db = _connectionMultiplexer.GetDatabase();
        var value = await db.StringGetAsync(id);
        return JsonSerializer.Deserialize<T>(value, _serializerOptions);
    }

    public async ValueTask DeleteAsync<T>(string id) {
        var db = _connectionMultiplexer.GetDatabase();
        await db.KeyDeleteAsync(id);
    }

    public async ValueTask StoreAsync<T>(string id, T entity) {
        var db = _connectionMultiplexer.GetDatabase();
        var serialized = JsonSerializer.Serialize(entity, _serializerOptions);
        await db.StringSetAsync(id, serialized);
    }

    public async ValueTask<IReadOnlyList<T>> GetAsync<T>() {
        var server = _connectionMultiplexer.GetServer(_connection.EndPoint);
        var db = _connectionMultiplexer.GetDatabase();

        IEnumerable<RedisKey> keys;
        if (typeof(T).Equals(typeof(UserData))) {
            keys = server.Keys(_connection.Database, "*userdata*");
        }
        else if (typeof(T).Equals(typeof(RoomData))) {
            keys = server.Keys(_connection.Database, "*roomdata*");
        }
        else {
            keys = server.Keys(_connection.Database, "*directorydata*");
        }

        var values = await Task.WhenAll(keys.Select(x => db.StringGetAsync(x)));
        var de = values.Select(x => JsonSerializer.Deserialize<T>(x, _serializerOptions));
        var final = (IReadOnlyList<T>)de;
        return final;
    }

    public ValueTask<int> CountAsync<T>() {
        var server = _connectionMultiplexer.GetServer(_connection.EndPoint);
        IEnumerable<RedisKey> keys;
        if (typeof(T).Equals(typeof(UserData))) {
            keys = server.Keys(_connection.Database, "*userdata*");
        }
        else if (typeof(T).Equals(typeof(RoomData))) {
            keys = server.Keys(_connection.Database, "*roomdata*");
        }
        else {
            keys = server.Keys(_connection.Database, "*directorydata*");
        }

        return ValueTask.FromResult(keys.Count());
    }

    public async ValueTask<bool> ExistsAsync<T>(string id) {
        var db = _connectionMultiplexer.GetDatabase();
        return await db.KeyExistsAsync(id);
    }

    public async ValueTask UpdateAsync<T>(string id, T before, T after)
        where T : class {
        var db = _connectionMultiplexer.GetDatabase();
        
        var update = typeof(T).Equals(typeof(UserData))
            ? UserData.Update(before as UserData, after as UserData) as T
            : typeof(T).Equals(typeof(RoomData))
            ? RoomData.Update(before as RoomData, after as RoomData) as T
            : DirectoryData.Update(before as DirectoryData, after as DirectoryData) as T;

        await db.StringSetAsync(id, JsonSerializer.Serialize(update, _serializerOptions));
    }
}