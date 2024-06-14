using System.Buffers;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Ankh.Handlers;
using Ankh.Models.Rework;

namespace Ankh;

public static class Extensions {
    private static readonly long[] StaffUserIds = [
        120862048, // IMVU_Offers
        87683724,  // IMVUSocial
        191931278, // IMVUMobile
        312470110, // Nola_AP
        11417,     //IMVU Badger
    ];
    
    private static readonly string[] Errors = [
        """
        "status": 401
        """,
        
        """
        "status": 404
        """
    ];
    
    private static readonly string[] UserAgents = [
        "Mozilla/5.0 (Win10; x64) Chrome/91.0.4472.124 Safari/537.36",
        "Mozilla/5.0 (Mac OS X 10_15_7) AppleWebKit/605.1.15 Safari/605.1.15",
        "Mozilla/5.0 (Linux x86_64) Chrome/91.0.4472.114 Safari/537.36",
        "Mozilla/5.0 (iPhone; CPU iOS 14_4_2) AppleWebKit/605.1.15 Mobile/15E148",
        "Mozilla/5.0 (Win10; rv:89.0) Gecko/20100101 Firefox/89.0"
    ];
    
    internal static readonly IDictionary<string, string> JsonKeyReplacements
        = new Dictionary<string, string> {
            { "owner_avatarname", "ownerUsername" },
            { "customers_avatar_name", "ownerUsername" },
            { "max_users", "capacity" },
            { "customers_id", "ownerId" },
            { "room_pid", "id" },
            { "room_instance_id", "id" },
            { "is_age_verified_only", "is_age_verified" },
            { "customers_id", "id" },
            { "avatar_name", "username" },
            { "avname", "username" },
            { "avatar_download_size", "size" },
            { "avpic", "profileImage" },
            { "legacy_cid", "id" },
            { "cid", "id" },
            { "avatar_image", "profileImage" },
            { "avpic_url", "profileImage" },
        };
    
    internal static Uri AsUri(this string str) {
        return new Uri(str);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="requestAction"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async ValueTask<JsonElement> GetJsonAsync(this HttpClient httpClient,
                                                            Action<HttpRequestMessage> requestAction) {
        using var requestMessage = new HttpRequestMessage();
        requestAction.Invoke(requestMessage);
        requestMessage.Headers.Add("User-Agent", UserAgents[Random.Shared.Next(UserAgents.Length - 1)]);
        
        using var responseMessage = await httpClient.SendAsync(requestMessage);
        
        if (responseMessage.StatusCode is HttpStatusCode.Forbidden or HttpStatusCode.Unauthorized) {
            throw new Exception($"Please use {nameof(UserHandler.LoginAsync)} before calling this method.");
        }
        
        var jsonContent = await responseMessage.Content.ReadAsStringAsync();
        foreach (var (key, value) in JsonKeyReplacements) {
            jsonContent = jsonContent.Replace(key, value);
        }
        
        var document = JsonDocument.Parse(jsonContent);
        if (document.RootElement.TryGetProperty("status", out var statusElement) &&
            statusElement.GetString()!.Equals("failure")) {
            throw new Exception(document.RootElement.GetProperty("message").GetString());
        }
        
        if (document.RootElement.TryGetProperty("http", out var httpElement) &&
            Errors.Any(x => httpElement.GetRawText().Contains(x))) {
            throw new Exception($"Please use {nameof(UserHandler.LoginAsync)} before calling this method.");
        }
        
        return document.RootElement;
    }
    
    internal static void WithAuthentication(this HttpRequestHeaders requestHeaders, UserLogin userLogin) {
        requestHeaders.Add("Cookie", $"osCsid={userLogin.SessionId}");
    }
    
    internal static JsonElement GetDernormalizedData(this JsonElement jsonElement) {
        return jsonElement
            .GetProperty("denormalized")
            .EnumerateObject()
            .First()
            .Value
            .GetProperty("data");
    }
    
    internal static T GetDernormalizedData<T>(this JsonElement jsonElement) {
        return GetDernormalizedData(jsonElement).Deserialize<T>()!;
    }
    
    internal static JsonElement MergeJson(JsonElement restJson, JsonElement phpJson) {
        restJson = GetDernormalizedData(restJson);
        
        var outputBuffer = new ArrayBufferWriter<byte>();
        using var writer = new Utf8JsonWriter(outputBuffer);
        writer.WriteStartObject();
        
        // .Where(p => !phpJson.TryGetProperty(p.Name, out _))
        foreach (var p in restJson.EnumerateObject()) {
            p.WriteTo(writer);
        }
        
        foreach (var p in phpJson.EnumerateObject()) {
            p.WriteTo(writer);
        }
        
        writer.WriteEndObject();
        writer.Flush();
        
        return JsonDocument.Parse(outputBuffer.WrittenMemory).RootElement;
    }
}