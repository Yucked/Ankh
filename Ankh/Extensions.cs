using System.Net;
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
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static long GetRandomUserId() {
        return StaffUserIds[Random.Shared.Next(StaffUserIds.Length - 1)];
    }
    
    public static void VerifyLogin(this UserSauce userSauce) {
        if (userSauce == default || string.IsNullOrWhiteSpace(userSauce.Auth)) {
            throw new Exception($"Please use {nameof(UserHandler.LoginAsync)} before calling this method.");
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="requestUrl"></param>
    /// <param name="cookie"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async ValueTask<T> GetRestModelAsync<T>(this HttpClient httpClient,
                                                          string requestUrl,
                                                          string cookie = "")
        where T : IRestModel {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        
        using var responseMessage = await httpClient.SendAsync(string.IsNullOrWhiteSpace(cookie)
            ? requestMessage.WithCookieSauce(cookie)
            : requestMessage);
        if (!responseMessage.IsSuccessStatusCode) {
            throw new Exception($"Failed to fetch {requestUrl} because of {responseMessage.ReasonPhrase}");
        }
        
        var restModel = await responseMessage.Content.ReadFromJsonAsync<RestModel>();
        if (restModel!.Status != "success") {
            throw new Exception($"API returned following status: {restModel.Status}");
        }
        
        return (T)restModel.Data;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="requestMessage"></param>
    /// <param name="cookie"></param>
    /// <returns></returns>
    public static HttpRequestMessage WithCookieSauce(this HttpRequestMessage requestMessage, string cookie) {
        // osCsid is session id - server sided can't spoof.
        requestMessage.Headers.Add("Cookie", $"osCsid={cookie}");
        return requestMessage;
    }
    
    public static void IsRequestSuccessful(HttpStatusCode statusCode, JsonElement rootElement) {
        var errors = new[] {
            """
            "status": 401
            """,
            
            """
            "status": 404
            """
        };
        
        if (statusCode is HttpStatusCode.Forbidden or HttpStatusCode.Unauthorized) {
            throw new Exception($"Please use {nameof(UserHandler.LoginAsync)} before calling this method.");
        }
        
        if (rootElement.TryGetProperty("status", out var statusElement) &&
            statusElement.GetString()!.Equals("failure")) {
            throw new Exception(rootElement.GetProperty("message").GetString());
        }
        
        if (rootElement.TryGetProperty("http", out var httpElement) &&
            errors.Any(x => httpElement.GetRawText().Contains(x))) {
            throw new Exception($"Please use {nameof(UserHandler.LoginAsync)} before calling this method.");
        }
    }
}