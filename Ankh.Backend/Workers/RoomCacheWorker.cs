namespace Ankh.Backend.Workers;

public sealed class RoomCacheWorker : BackgroundService {
    protected override Task ExecuteAsync(CancellationToken stoppingToken) {
        return Task.CompletedTask;
    }
}