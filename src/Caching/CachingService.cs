using System.Collections.Concurrent;
using Ankh.Data;

namespace Ankh.Caching;

public sealed class CachingService : BackgroundService {
    private readonly UserCacher _userCacher;
    private readonly RoomCacher _roomCacher;
    private readonly DirectoryCacher _directoryCacher;
    private readonly ILogger _logger;
    private readonly ConcurrentQueue<string> _urls;
    private readonly Database _database;

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
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        _logger.LogInformation($"{nameof(CachingService)} is starting...");
        stoppingToken.Register(() => _logger.LogDebug("RoomCaching task is stopping..."));
        _ = UpdateDatabaseAsync(stoppingToken);

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

    private async Task UpdateDatabaseAsync(CancellationToken stoppingToken) {
        while (!stoppingToken.IsCancellationRequested) {
            await Parallel.ForEachAsync(_userCacher.Cache, stoppingToken,
                async (data, _) => {
                    var oldData = await _database.GetAsync<UserData>(data.Id);
                    await _database.UpdateAsync(data.Id, UserData.Update(oldData, data));
                });

            await Parallel.ForEachAsync(_roomCacher.Cache, stoppingToken,
                async (data, _) => {
                    var oldData = await _database.GetAsync<RoomData>(data.Id);
                    await _database.UpdateAsync(data.Id, RoomData.Update(oldData, data));
                });

            await Parallel.ForEachAsync(_directoryCacher.Cache, stoppingToken,
                async (data, _) => {
                    var oldData = await _database.GetAsync<DirectoryData>(data.Id);
                    await _database.UpdateAsync(data.Id, DirectoryData.Update(oldData, data));
                });

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}