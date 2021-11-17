using Ankh.Data;
using ServiceStack.Redis;

namespace Ankh;

public sealed class Database {
    private readonly IRedisClientsManagerAsync _clientsManager;
    private readonly ILogger _logger;

    public Database(IRedisClientsManagerAsync clientsManager,
        ILogger<Database> logger) {
        _clientsManager = clientsManager;
        _logger = logger;
    }

    public async ValueTask<T> GetAsync<T>(string id) {
        await using var client = await _clientsManager.GetReadOnlyClientAsync();
        var dataType = client.As<T>();
        var document = await dataType.GetByIdAsync(id);
        return document;
    }

    public async ValueTask DeleteAsync<T>(string id) {
        await using var client = await _clientsManager.GetClientAsync();
        var dataType = client.As<T>();
        await dataType.DeleteByIdAsync(id);
    }

    public async ValueTask StoreAsync<T>(T entity) {
        await using var client = await _clientsManager.GetClientAsync();
        var dataType = client.As<T>();
        await dataType.StoreAsync(entity);
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