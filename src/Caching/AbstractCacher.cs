using System.Collections.Concurrent;

namespace Ankh.Caching;

public abstract record AbstractCacher<T>(ILogger<T> Logger, HttpClient HttpClient) {
    protected readonly ILogger<T> Logger = Logger;
    protected readonly HttpClient HttpClient = HttpClient;
    public ConcurrentDictionary<string, T> Cache = new();

    protected void AddToCache(string id, T item) {
        Cache.TryAdd(id, item);
    }
}