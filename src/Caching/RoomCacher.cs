using Ankh.Data;

namespace Ankh.Caching;

public record RoomCacher(ILogger<RoomData> Logger, HttpClient HttpClient)
    : AbstractCacher<RoomData>(Logger, HttpClient) {
    public async Task CacheRoomAsync(string url) {
        using var responseMessage = await HttpClient.GetAsync(url);
        if (!responseMessage.IsSuccessStatusCode) {
            Logger.LogWarning("{ReasonPhrase}", responseMessage.ReasonPhrase);
            return;
        }

        using var content = responseMessage.Content;
        var byteData = await content.ReadAsByteArrayAsync();
        try {
            var room = RoomData.ToRoom(byteData, url);
            AddToCache(room.Id, room);
        }
        catch (Exception exception) {
            Logger.LogCritical("{Message} {exception}", exception.Message, exception);
        }
    }
}