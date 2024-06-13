using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Ankh.Handlers;
using Ankh.Models.Rest;

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
    
    public static Uri AsUri(this string str) {
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
        
        var document = await JsonDocument.ParseAsync(await responseMessage.Content.ReadAsStreamAsync());
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
    
    public static void IsRequestSuccessful(HttpStatusCode statusCode, JsonElement rootElement) {
        if (statusCode is HttpStatusCode.Forbidden or HttpStatusCode.Unauthorized) {
            throw new Exception($"Please use {nameof(UserHandler.LoginAsync)} before calling this method.");
        }
        
        if (rootElement.TryGetProperty("status", out var statusElement) &&
            statusElement.GetString()!.Equals("failure")) {
            throw new Exception(rootElement.GetProperty("message").GetString());
        }
        
        if (rootElement.TryGetProperty("http", out var httpElement) &&
            Errors.Any(x => httpElement.GetRawText().Contains(x))) {
            throw new Exception($"Please use {nameof(UserHandler.LoginAsync)} before calling this method.");
        }
    }
    
    internal static void WithAuthentication(this HttpRequestHeaders requestHeaders, UserSauce userSauce) {
        requestHeaders.Add("Cookie", $"osCsid={userSauce.Auth}");
    }
    
    internal static T GetDernormalizedData<T>(this JsonElement jsonElement) {
        return jsonElement
            .GetProperty("denormalized")
            .EnumerateObject()
            .First()
            .Value
            .GetProperty("data")
            .Deserialize<T>()!;
    }
}