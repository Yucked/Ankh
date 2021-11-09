namespace Ankh.Caching;

public abstract record AbstractCacher<T> {
    protected ILogger<T> _logger;
    protected HttpClient _httpClient;
    public HashSet<T> Cache { get; }

    public AbstractCacher(ILogger<T> logger, HttpClient httpClient) {
        _logger = logger;
        _httpClient = httpClient;
        Cache = new HashSet<T>();
    }

    public void AddToCache(T item) {
        Cache.Add(item);
    }
}