namespace Ankh.Caching;

public abstract record AbstractCacher<T>(ILogger<T> Logger, HttpClient HttpClient) {
    protected readonly ILogger<T> Logger = Logger;
    protected readonly HttpClient HttpClient = HttpClient;
    public HashSet<T> Cache { get; } = new();

    protected void AddToCache(T item) {
        Cache.Add(item);
    }
}