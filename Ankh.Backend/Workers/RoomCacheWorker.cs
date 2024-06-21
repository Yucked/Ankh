using Ankh.Handlers;
using Nito.AsyncEx;

namespace Ankh.Backend.Workers;

public sealed class RoomCacheWorker(
    Spyder spyder,
    RoomHandler roomHandler,
    Database database,
    ILogger<RoomCacheWorker> logger) : BackgroundService {
    private const string ROOMS_URL = "https://www.imvu.com/rooms";
    private readonly HashSet<string> _roomIds = new();
    
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
                
                await Parallel.ForEachAsync(_roomIds, stoppingToken, async (x, _) => {
                    var roomModel = await roomHandler.GetRoomByIdAsync(Database.RandomLogin, x);
                    await database.SaveAsync(roomModel);
                });
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
        
        try {
            bool goToNext;
            var requestUrl = $"{ROOMS_URL}/{href}";
            
            do {
                var page = (await spyder.RequestPageAsync(requestUrl))!;
                
                // Get next page
                var nextPage = (await page.QuerySelectorAllAsync("div[align] > a")).LastOrDefault();
                if (nextPage == null) {
                    logger.LogError("Unable to find next page element for {}", page.Url);
                    return;
                }
                
                goToNext = await nextPage.TextContentAsync() == "Previous";
                requestUrl = $"{ROOMS_URL}{await nextPage.GetAttributeAsync("href")}";
                
                // Parse links on current page
                var roomDirectoryElement = await page.QuerySelectorAllAsync("a.roomdirectory-link");
                var roomHrefElement = await roomDirectoryElement
                    .Select(x => x.GetAttributeAsync("href"))
                    .WhenAll();
                
                var roomIds = roomHrefElement.Select(x => x![(x.IndexOf('=') + 1)..]);
                _roomIds.UnionWith(roomIds);
            } while (goToNext);
        }
        catch (Exception exception) {
            logger.LogError("{exception}", exception);
        }
    }
}