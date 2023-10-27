using System.Collections.Concurrent;
using Ankh.Handlers;
using Ankh.Redis;
using Ankh.Redis.Interfaces;

namespace Ankh.Caching;

public sealed class CachingService : BackgroundService {
    private readonly DirectoryCacher _directoryCacher;
    private readonly RoomHandler _roomHandler;
    private readonly ILogger<CachingService> _logger;
    private readonly ConcurrentQueue<string> _urls;
    private readonly RedisClientManager _clientManager;
    private readonly PeriodicTimer _periodicTimer;

    public CachingService(DirectoryCacher directoryCacher,
                          ILogger<CachingService> logger,
                          RedisClientManager clientManager) {
        _directoryCacher = directoryCacher;
        _logger = logger;
        _clientManager = clientManager;

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
        where T : IRedisEntity {
        await Parallel.ForEachAsync(cacher.Cache, LoopAsync);

        async ValueTask LoopAsync(KeyValuePair<string, T> kvp,
                                  CancellationToken cancellationToken) {
            cacher.Cache.TryRemove(kvp.Key, out var value);

            var client = _clientManager.For<T>();
            if (await client.ExistsAsync(kvp.Key)) { }

            if (!await client.ExistsAsync(kvp.Key)) {
                await client.AddAsync(value);
                return;
            }

            var oldData = await client.GetAsync(kvp.Key);
            client.AddAsync(kvp.Key, oldData, value);
        }
    }
}