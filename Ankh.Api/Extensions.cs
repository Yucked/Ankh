using System.Net.Http.Json;
using Ankh.Api.Models.Rest;

namespace Ankh.Api;

public static class Extensions {
    private static readonly long[] StaffUserIds = [
        120862048, // IMVU_Offers
        87683724,  // IMVUSocial
        191931278, // IMVUMobile
        312470110, // Nola_AP
        11417, //IMVU Badger
    ];
    
    public static long GetRandomUserId() {
        return StaffUserIds[Random.Shared.Next(StaffUserIds.Length - 1)];
    }
    
    public static async ValueTask<T> GetRestModelAsync<T>(this HttpClient httpClient, string requestUrl)
        where T : IRestModel {
        using var responseMessage = await httpClient.GetAsync(requestUrl);
        if (!responseMessage.IsSuccessStatusCode) {
            throw new Exception($"Failed to fetch {requestUrl} because of {responseMessage.ReasonPhrase}");
        }
        
        var restModel = await responseMessage.Content.ReadFromJsonAsync<RestModel>();
        if (restModel!.Status != "success") {
            throw new Exception($"API returned following status: {restModel.Status}");
        }
        
        return (T)restModel.Data;
    }
    
    public static HttpRequestMessage AddLoginCookie(this HttpRequestMessage requestMessage) {
        // osCsid is session id - server sided can't spoof.
        requestMessage.Headers.Add("Cookie", "osCsid=");
        return requestMessage;
    }
}