using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Web;
using Ankh.Api.Models;
using Ankh.Api.Models.Rest;
using Microsoft.Extensions.Logging;

namespace Ankh.Api.Handlers;

[SuppressMessage("Performance", "CA1822:Mark members as static")]
public sealed class ProductHandler(
    ILogger<ProductHandler> logger,
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
                (x!.Replace("avatar", string.Empty), queryParams[x]!
                     .Split(';')
                     .Select(id => id.Contains('x') ? id[..id.IndexOf('x')] : id))
            )
            .ToDictionary(x => x.Item1, y => y.Item2.ToArray());
        return ValueTask.FromResult<IDictionary<string, string[]>>(sceneItems);
    }
    
    /// <summary>
    /// Fetches product information from API
    /// </summary>
    /// <param name="productId"></param>
    public ValueTask<RestProductModel> GetProductByIdAsync(int productId) {
        if (productId <= 0) {
            throw new ArgumentException("Can't be less than or equal to 0.", nameof(productId));
        }
        
        try {
            return httpClient.GetRestModelAsync<RestProductModel>($"https://api.imvu.com/product/product-{productId}");
        }
        catch (Exception exception) {
            logger.LogError("{exception.Message}", exception.Message);
            throw;
        }
    }
    
    /// <summary>
    /// Fetches all products from creator's shop.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="userSauce"></param>
    /// <returns></returns>
    public async ValueTask<IReadOnlyCollection<RestProductModel>> GetProductsByCreatorAsync(
        string username,
        UserSauce userSauce = default) {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        
        var requestMessage = new HttpRequestMessage(HttpMethod.Get,
            $"https://api.imvu.com/product?creator={username}&limit=1");
        
        try {
            var responseMessage = await httpClient.SendAsync(userSauce != default
                ? requestMessage.WithCookieSauce(userSauce.Auth)
                : requestMessage);
            var document = await JsonDocument.ParseAsync(await responseMessage.Content.ReadAsStreamAsync());
            var totalCount = document
                .RootElement
                .GetProperty("denormalized")
                .GetProperty(document.RootElement.GetProperty("id").GetString()!)
                .GetProperty("data")
                .GetProperty("total_count")
                .GetInt32();
            
            requestMessage = new HttpRequestMessage(HttpMethod.Get,
                $"https://api.imvu.com/product?creator={username}&limit={totalCount}");
            
            responseMessage =
                await httpClient.SendAsync(userSauce != default
                    ? requestMessage.WithCookieSauce(userSauce.Auth)
                    : requestMessage);
            document = await JsonDocument.ParseAsync(await responseMessage.Content.ReadAsStreamAsync());
            return document
                .RootElement
                .GetProperty("denormalized")
                .EnumerateObject()
                .Where(x =>
                    !x.Name.Equals($"{responseMessage.RequestMessage!.RequestUri}",
                        StringComparison.CurrentCultureIgnoreCase))
                .Select(x => x.Value.GetProperty("data").Deserialize<RestProductModel>())
                .ToArray()!;
        }
        catch (Exception exception) {
            logger.LogError("{exception.Message}", exception.Message);
            throw;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userSauce"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public ValueTask<RestCreatorModel> GetCreatorInformationAsync(UserSauce userSauce, int userId) {
        if (userId <= 0) {
            throw new ArgumentException("Can't be less than or equal to 0.", nameof(userId));
        }
        
        try {
            return httpClient.GetRestModelAsync<RestCreatorModel>(
                $"https://api.imvu.com/creator/creator-{userId}",
                userSauce.Auth);
        }
        catch (Exception exception) {
            logger.LogError("{exception.Message}", exception.Message);
            throw;
        }
    }
}