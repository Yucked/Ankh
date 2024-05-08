using System.Net.Http.Json;
using Ankh.Api.Models;

namespace Ankh.Api;

public static class Extensions {
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
}