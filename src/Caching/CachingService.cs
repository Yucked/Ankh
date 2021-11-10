using System.Collections.Concurrent;

namespace Ankh.Caching;

public sealed class CachingService : BackgroundService {
    private readonly UserCacher _userCacher;
    private readonly RoomCacher _roomCacher;
    private readonly DirectoryCacher _directoryCacher;
    private readonly ILogger _logger;
    private readonly ConcurrentQueue<string> _urls;

    public CachingService(UserCacher userCacher,
        RoomCacher roomCacher,
        DirectoryCacher directoryCacher,
        ILogger<CachingService> logger) {
        _userCacher = userCacher;
        _roomCacher = roomCacher;
        _directoryCacher = directoryCacher;
        _logger = logger;

        _urls = new ConcurrentQueue<string>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        _logger.LogInformation($"{nameof(CachingService)} is starting...");
        stoppingToken.Register(() => _logger.LogDebug("RoomCaching task is stopping..."));

        while (!stoppingToken.IsCancellationRequested) {
            await _directoryCacher.CacheDirectoriesAsync(stoppingToken);
            _logger.LogInformation("Total rooms: {Count}",
                _directoryCacher.Cache.Sum(x => x.Records.Count));

            Parallel.ForEach(
                _directoryCacher.Cache.SelectMany(x => x.Records),
                url => _urls.Enqueue($"R::{url}"));

            await ProcessUrlsAsync(stoppingToken);
        }

        _logger.LogWarning($"{nameof(CachingService)} has stopped!");
    }

    private async Task ProcessUrlsAsync(CancellationToken stoppingToken) {
        while (_urls.TryDequeue(out var url) && !stoppingToken.IsCancellationRequested) {
            switch (url[0]) {
                case 'R':
                    await _roomCacher.CacheRoomAsync(url[3..]);
                    var lastRoom = _roomCacher.Cache.LastOrDefault();
                    _logger.LogInformation("Room cached -> {name}", lastRoom.Name);
                    if (lastRoom.UserHistory.Count == 0) {
                        continue;
                    }

                    foreach (var user in lastRoom.UserHistory.Keys) {
                        _urls.Enqueue($"U::{user}");
                    }
                    break;

                case 'U':
                    var userId = await _userCacher.GetIdAsync(url[3..]);
                    await _userCacher.CacheUserAsync(userId);
                    _logger.LogInformation("User cached -> {userId}", userId);
                    break;
            }
        }
    }
}