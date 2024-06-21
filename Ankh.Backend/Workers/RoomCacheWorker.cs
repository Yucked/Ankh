using Ankh.Handlers;
using Nito.AsyncEx;

namespace Ankh.Backend.Workers;

public sealed class RoomCacheWorker(
    Spyder spyder,
    RoomHandler roomHandler,
    Database database,
    ILogger<RoomCacheWorker> logger) : BackgroundService {
    private const string ROOMS_URL = "https://www.imvu.com/rooms";
    private readonly HashSet<string> _failedUrls = new();
    private readonly SemaphoreSlim _semaphoreSlim = new(2, 5);
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        var letterTracker = string.Empty;
        
        while (!stoppingToken.IsCancellationRequested) {
            try {
                var requestUrl = string.IsNullOrWhiteSpace(letterTracker)
                    ? ROOMS_URL
                    : $"{ROOMS_URL}/?letter={letterTracker}";
                
                var page = await spyder.RequestPageAsync(requestUrl);
                var elements = await page!.QuerySelectorAllAsync("span.letter-link > a");
                
                await Parallel.ForEachAsync(
                    await elements.Select(x => x.GetAttributeAsync("href")).WhenAll(),
                    stoppingToken,
                    HandleLetterAsync);
                
                await GetRoomsInformationAsync(_failedUrls, stoppingToken);
            }
            catch (Exception exception) {
                logger.LogError("{exception}", exception);
            }
        }
    }
    
    private async ValueTask HandleLetterAsync(string? href, CancellationToken stoppingToken) {
        if (string.IsNullOrWhiteSpace(href)) {
            return;
        }
        
        await _semaphoreSlim.LockAsync(stoppingToken);
        var requestUrl = string.Empty;
        try {
            foreach (var i in Enumerable.Range(0, 150)) {
                requestUrl = $"{ROOMS_URL}/{href}&page={i}";
                var page = (await spyder.RequestPageAsync(requestUrl))!;
                
                var roomDirectoryElement = await page.QuerySelectorAllAsync("a.roomdirectory-link");
                if (!roomDirectoryElement.Any()) {
                    logger.LogWarning("No more links to parse on {}. Skipping to next letter.", page.Url);
                    break;
                }
                
                var roomHrefElement = await roomDirectoryElement
                    .Select(x => x.GetAttributeAsync("href"))
                    .WhenAll();
                
                var roomIds = roomHrefElement.Select(x => x![(x.IndexOf('=') + 1)..]);
                await GetRoomsInformationAsync(roomIds, stoppingToken);
            }
        }
        catch (Exception e) {
            logger.LogError("{e}", e);
            _failedUrls.Add(requestUrl);
        }
    }
    
    private async ValueTask GetRoomsInformationAsync(IEnumerable<string> roomIds, CancellationToken stoppingToken) {
        await _semaphoreSlim.LockAsync(stoppingToken);
        await Parallel.ForEachAsync(roomIds, stoppingToken, async (x, _) => {
            var roomModel = await roomHandler.GetRoomByIdAsync(Database.RandomLogin, x);
            await database.SaveAsync(roomModel);
        });
    }
}