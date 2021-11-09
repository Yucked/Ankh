using Ankh.Data;

namespace Ankh.Caching;
public record RoomCacher : AbstractCacher<RoomData> {
    public RoomCacher(ILogger<RoomData> logger, HttpClient httpClient)
        : base(logger, httpClient) { }

    public async Task CacheRoomAsync(string url) {
        using var responseMessage = await _httpClient.GetAsync(url);
        if (!responseMessage.IsSuccessStatusCode) {
            _logger.LogWarning("{ReasonPhrase}", responseMessage.ReasonPhrase);
            return;
        }

        using var content = responseMessage.Content;
        var byteData = await content.ReadAsByteArrayAsync();
        try {
            var room = RoomData.ToRoom(byteData, url);
            AddToCache(room);
        }
        catch (Exception exception) {
            _logger.LogCritical("{Message} {exception}", exception.Message, exception);
        }
    }
}