using System.Text;
using System.Text.Json;
using Ankh.Data;
using Ankh.Redis;

namespace Ankh.Handlers;

public class RoomHandler {
    private static readonly ReadOnlyMemory<byte> RoomInfoVar
        = "roomInfo ="u8.ToArray();

    private static readonly ReadOnlyMemory<byte> ScriptEnd
        = "</script>"u8.ToArray();

    private readonly RedisClientManager _redisClientManager;
    private readonly ILogger<UserHandler> _logger;
    private readonly HttpClient _httpClient;

    public RoomHandler(RedisClientManager redisClientManager, ILogger<UserHandler> logger, HttpClient httpClient) {
        _redisClientManager = redisClientManager;
        _logger = logger;
        _httpClient = httpClient;
    }

    public static RoomData Update(RoomData before, RoomData after) {
        var updated = before.Update(after);
        foreach (var (user, time) in after.History) {
            updated.History[user] = time;
        }

        return updated;
    }

    public async Task CacheRoomAsync(string url) {
        using var requestMessage = new HttpRequestMessage {
            Method = HttpMethod.Get,
            RequestUri = new Uri(url)
        };
        using var responseMessage = await _httpClient.SendAsync(requestMessage);
        if (!responseMessage.IsSuccessStatusCode) {
            _logger.LogWarning("{ReasonPhrase}", responseMessage.ReasonPhrase);
            return;
        }

        using var content = responseMessage.Content;
        var byteData = await content.ReadAsByteArrayAsync();
        try {
            static string GetJson(Span<byte> data) {
                var slice = data[(data.IndexOf(RoomInfoVar.Span) + RoomInfoVar.Length)..];
                slice = slice[..slice.IndexOf(ScriptEnd.Span)];

                return Encoding.UTF8
                    .GetString(slice)
                    .Replace("'", "\"");
            }

            var document = JsonDocument.Parse(GetJson(byteData)).RootElement;
            var participants = new Dictionary<string, DateTimeOffset>();
            foreach (var name in document.GetProperty("participants")
                         .EnumerateArray()
                         .Select(participant => participant.GetProperty("name").GetString())) {
                participants[name!] = DateTimeOffset.UtcNow;
            }

            var roomData = new RoomData(
                document.Get<string>("roomInstanceId"),
                DateOnly.FromDateTime(DateTime.Now),
                document.Get<string>("room_name", JsonType.Decode),
                string.IsNullOrWhiteSpace(document.GetProperty("ap").GetString()),
                string.IsNullOrWhiteSpace(document.GetProperty("vip").GetString()),
                document.Get<string>("owner"),
                document.Get<string>("desc", JsonType.Decode),
                document.Get<int>("max", JsonType.StrToInt),
                document.Get<int>("count", JsonType.StrToInt),
                document.Get<string>("img"),
                document.GetProperty("whitelist_rating").GetInt32(),
                url.Decode(),
                document.GetProperty("show_ap_name_only").GetBoolean(),
                participants);

            await _redisClientManager.For<RoomData>().AddAsync(roomData);
        }
        catch (Exception exception) {
            _logger.LogCritical("{Message} {exception}", exception.Message, exception);
        }
    }

    public async Task GetApiRoomInfoAsync(string roomId) {
        ArgumentException.ThrowIfNullOrEmpty(roomId);
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get,
            $"http://client-dynamic.imvu.com/api/rooms/room_info.php?room_id={roomId}");
    }
}