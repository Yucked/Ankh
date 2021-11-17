using Ankh.Data;
using ServiceStack.Redis;
using StackExchange.Redis;
using System.Text.Json;

namespace Ankh;

public sealed class Database {
    private readonly IRedisClientsManagerAsync _clientsManager;
    private readonly ConnectionMultiplexer _connectionMultiplexer;
    private readonly ILogger _logger;

    public Database(IRedisClientsManagerAsync clientsManager, ConnectionMultiplexer connectionMultiplexer,
        ILogger<Database> logger) {
        _connectionMultiplexer = connectionMultiplexer;
        _clientsManager = clientsManager;
        _logger = logger;
    }

    public async ValueTask<T> GetAsync<T>(string id) {
        var db = _connectionMultiplexer.GetDatabase();
        var value = await db.StringGetAsync(id);
        return JsonSerializer.Deserialize<T>(value);
    }

    public async ValueTask DeleteAsync<T>(string id) {
        var db = _connectionMultiplexer.GetDatabase();
        await db.KeyDeleteAsync(id);
    }

    public async ValueTask StoreAsync<T>(string id, T entity) {
        var db = _connectionMultiplexer.GetDatabase();
        var serialized = JsonSerializer.Serialize(entity);
        await db.StringSetAsync(id, serialized);
    }

    public async ValueTask<IReadOnlyList<T>> GetAsync<T>() {
        await using var client = await _clientsManager.GetReadOnlyClientAsync();
        var dataType = client.As<T>();
        var dataList = await dataType.GetAllAsync();
        return dataList as IReadOnlyList<T>;
    }

    public async ValueTask<int> CountAsync<T>() {
        var documents = await GetAsync<T>();
        var document = documents.Count > 1 ? documents[0] : default;
        return document switch {
            UserData => documents.Count,
            RoomData => documents.Count,
            DirectoryData => documents.Cast<DirectoryData>().Sum(x => x.Records.Count),
            _ => 0
        };
    }

    public async ValueTask<bool> ExistsAsync<T>(string id) {
        await using var client = await _clientsManager.GetReadOnlyClientAsync();
        var dataType = client.As<T>();
        var exists = await dataType.ContainsKeyAsync(id);
        return exists;
    }

    public async ValueTask UpdateAsync<T>(string id, T update) {
        await using var client = await _clientsManager.GetReadOnlyClientAsync();
        var dataType = client.As<T>();
        await dataType.StoreAsync(update);
    }
}