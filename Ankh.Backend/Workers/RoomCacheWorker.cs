namespace Ankh.Backend.Workers;

public sealed class RoomCacheWorker(Spyder spyder) : BackgroundService {
    private const string ROOMS_URL = "https://www.imvu.com/rooms";
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        var letterTracker = string.Empty;
        
        while (!stoppingToken.IsCancellationRequested) {
            var requestUrl = string.IsNullOrWhiteSpace(letterTracker)
                ? ROOMS_URL
                : $"{ROOMS_URL}/?letter={letterTracker}";
            
            var page = await spyder.RequestPageAsync(requestUrl);
            var elements = await page.QuerySelectorAllAsync("span.letter-link > a");
            var hrefs = await Task.WhenAll(elements.Select(x => x.GetAttributeAsync("href")));
            
            await Parallel.ForEachAsync(hrefs, stoppingToken, ParseRoomInfoAsync);
        }
    }
    
    private async ValueTask ParseRoomInfoAsync(string? href, CancellationToken token) {
        if (string.IsNullOrWhiteSpace(href)) {
            return;
        }
        
        var page = await spyder.RequestPageAsync($"{ROOMS_URL}/{href}");
        var nextPage = (await page.QuerySelectorAllAsync("div[align] > a")).Last();
        
        var rooms = Task.WhenAll((await page.QuerySelectorAllAsync("a.roomdirectory-link"))
            .Select(x => x.GetAttributeAsync("href")));
        
        
    }
}