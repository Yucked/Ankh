using System.Collections.Concurrent;

namespace Ankh.Caching;

public sealed class CachingService : BackgroundService {
    private readonly UserCacher _userCacher;
    private readonly RoomCacher _roomCacher;
    private readonly DirectoryCacher _directoryCacher;
    private readonly ILogger _logger;
    private readonly ConcurrentQueue<string> _urls;
    private readonly Database _database;
    private readonly PeriodicTimer _periodicTimer;

    public CachingService(UserCacher userCacher,
                          RoomCacher roomCacher,
                          DirectoryCacher directoryCacher,
                          ILogger<CachingService> logger,
                          Database database) {
        _userCacher = userCacher;
        _roomCacher = roomCacher;
        _directoryCacher = directoryCacher;
        _logger = logger;
        _database = database;

        _urls = new ConcurrentQueue<string>();
        _periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(15));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        _logger.LogInformation($"{nameof(CachingService)} is starting...");
        stoppingToken.Register(() => _logger.LogDebug("RoomCaching task is stopping..."));
        _ = UpdateDatabaseAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested) {
            await _directoryCacher.CacheDirectoriesAsync(stoppingToken);
            _logger.LogInformation("Total rooms: {Count}",
                _directoryCacher.Cache.Sum(x => x.Value.Records.Count));

            Parallel.ForEach(
                _directoryCacher.Cache.SelectMany(x => x.Value.Records).Distinct(),
                url => _urls.Enqueue($"R::{url}"));

            await ProcessUrlsAsync(stoppingToken);
        }

        _logger.LogWarning($"{nameof(CachingService)} has stopped!");
    }

    private async Task ProcessUrlsAsync(CancellationToken stoppingToken) {
        while (_urls.TryDequeue(out var url) && !stoppingToken.IsCancellationRequested) {
            try {
                switch (url[0]) {
                    case 'R':
                        await _roomCacher.CacheRoomAsync(url[3..]);
                        var (id, lastRoom) = _roomCacher.Cache.FirstOrDefault(x => x.Value.Url == url[3..]);
                        if (lastRoom == null) {
                            continue;
                        }

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
                        break;
                }
            }
            catch (Exception exception) {
                _logger.LogError("Error on url: {url}", url);
                _logger.LogError("{Message}{NewLine}{StackTrace}",
                    exception.Message, Environment.NewLine, exception.StackTrace);
            }
        }
    }

    private async Task UpdateDatabaseAsync(CancellationToken stoppingToken) {
        while (await _periodicTimer.WaitForNextTickAsync(stoppingToken)) {
            await ParallelProcessAsync(_userCacher);
            await ParallelProcessAsync(_roomCacher);
            await ParallelProcessAsync(_directoryCacher);
        }
    }

    private async Task ParallelProcessAsync<T>(AbstractCacher<T> cacher)
        where T : class {
        await Parallel.ForEachAsync(cacher.Cache, LoopAsync);

        async ValueTask LoopAsync(KeyValuePair<string, T> kvp,
            CancellationToken cancellationToken) {
            cacher.Cache.TryRemove(kvp.Key, out var value);

            if (!await _database.ExistsAsync<T>(kvp.Key)) {
                await _database.StoreAsync(kvp.Key, value);
                return;
            }

            var oldData = await _database.GetAsync<T>(kvp.Key);
            await _database.UpdateAsync(kvp.Key, oldData, value);
        }
    }
}