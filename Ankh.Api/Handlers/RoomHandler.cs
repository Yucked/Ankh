using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Web;
using Ankh.Api.Models;
using Microsoft.Extensions.Logging;

namespace Ankh.Api.Handlers;

[SuppressMessage("Performance", "CA1822:Mark members as static")]
public sealed class RoomHandler(
    ILogger<RoomHandler> logger,
    HttpClient httpClient) {
    /// <summary>
    /// Displays all items in scene (room) alongside user's outfits regardless of if they are hidden.
    /// </summary>
    /// <param name="roomProductsUrl"></param>
    /// <returns>Key, Value where Key is user/room id and value is list of items.</returns>
    public ValueTask<IDictionary<string, string[]>> GetProductsInSceneAsync(string roomProductsUrl) {
        ArgumentException.ThrowIfNullOrWhiteSpace(roomProductsUrl);
        
        var queryParams = HttpUtility.ParseQueryString(new Uri(roomProductsUrl).Query);
        var sceneItems = queryParams
            .AllKeys
            .Select(x =>
                (x!.Replace("avatar", string.Empty), queryParams[x]!.Split(';'))
            )
            .ToDictionary(x => x.Item1, y => y.Item2);
        return ValueTask.FromResult<IDictionary<string, string[]>>(sceneItems);
    }
    
    /// <summary>
    /// Fetches product information from API
    /// </summary>
    /// <param name="productId"></param>
    public async ValueTask<ProductModel> GetProductByIdAsync(int productId) {
        if (productId <= 0) {
            throw new ArgumentException("Can't be less than or equal to 0.", nameof(productId));
        }
        
        var apiUrl = $"https://api.imvu.com/product/product-{productId}";
        using var responseMessage = await httpClient.GetAsync(apiUrl);
        if (!responseMessage.IsSuccessStatusCode) {
            logger.LogError("Failed product fetch {}", productId);
            throw new Exception("");
        }
        
        var restModel = await responseMessage.Content.ReadFromJsonAsync<RestModel>();
        if (restModel!.Status != "success") {
            throw new Exception("");
        }
        
        return (ProductModel)restModel.Data;
    }
}