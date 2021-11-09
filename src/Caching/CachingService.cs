namespace Ankh.Caching;

public sealed class CachingService : BackgroundService {
    private readonly UserCacher _userCacher;
    private readonly RoomCacher _roomCacher;
    private readonly DirectoryCacher _directoryCacher;

    private readonly ILogger _logger;
    private readonly HashSet<string> _urls;

    public CachingService(UserCacher userCacher,
        RoomCacher roomCacher,
        DirectoryCacher directoryCacher,
        ILogger<CachingService> logger) {
        _userCacher = userCacher;
        _roomCacher = roomCacher;
        _directoryCacher = directoryCacher;
        _logger = logger;

        _urls = new HashSet<string>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        _logger.LogInformation($"{nameof(CachingService)} is starting...");
        stoppingToken.Register(() => _logger.LogDebug("RoomCaching task is stopping..."));

        while (!stoppingToken.IsCancellationRequested) {
            await _directoryCacher.CacheDirectoriesAsync(stoppingToken);
            _logger.LogInformation("Total rooms: {Count}", _directoryCacher.Cache.Sum(x => x.Records.Count));

            foreach (var url in _directoryCacher.Cache.SelectMany(x => x.Records)) {
                if (TryAddToRequestBucket(url)) {
                    continue;
                }

                await ProcessUrlsAsync();
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        _logger.LogWarning($"{nameof(CachingService)} has stopped!");
    }

    private bool TryAddToRequestBucket(string url) {
        if (_urls.Contains(url)) {
            return true;
        }

        if (_urls.Count > 100) {
            return false;
        }

        _urls.Add(url);
        return true;
    }

    private async Task ProcessUrlsAsync() {
        foreach (var url in _urls) {
            if (url.StartsWith("USER::")) {
                var userId = await _userCacher.GetIdAsync(url[6..]);
                await _userCacher.CacheUserAsync(userId);
            }


            await _roomCacher.CacheRoomAsync(url);
            var lastRoom = _roomCacher.Cache.LastOrDefault();
            if (lastRoom.UserHistory.Count == 0) {
                return;
            }

            foreach (var user in lastRoom.UserHistory.Keys) {
                _urls.Add($"USER::{user}");
            }
        }
    }
}