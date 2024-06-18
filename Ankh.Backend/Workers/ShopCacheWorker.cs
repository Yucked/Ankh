namespace Ankh.Backend.Workers;

public sealed class ShopCacheWorker : BackgroundService {
    protected override Task ExecuteAsync(CancellationToken stoppingToken) {
        return Task.CompletedTask;
    }
}