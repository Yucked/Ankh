using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Web;
using Ankh.Api.Models;
using Ankh.Models.Enums;
using Ankh.Models.Queries;
using Ankh.Models.Rest;
using Ankh.Models.Rework;
using Microsoft.Extensions.Logging;

namespace Ankh.Handlers;

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
    public async ValueTask<RestProductModel> GetProductByIdAsync(int productId) {
        if (productId <= 0) {
            throw new ArgumentException("Can't be less than or equal to 0.", nameof(productId));
        }
        
        try {
            var jsonElement = await httpClient.GetJsonAsync(x => {
                x.RequestUri = $"https://api.imvu.com/product/product-{productId}".AsUri();
            });
            
            return jsonElement.GetDernormalizedData<RestProductModel>();
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
    /// <param name="userLogin"></param>
    /// <returns></returns>
    public async ValueTask<IReadOnlyCollection<RestProductModel>> GetProductsByCreatorAsync(
        string username,
        UserLogin userLogin) {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        
        try {
            var jsonElement = await httpClient.GetJsonAsync(x => {
                x.RequestUri = $"https://api.imvu.com/product?creator={username}&limit=1".AsUri();
                x.Headers.WithAuthentication(userLogin);
            });
            
            var totalCount = jsonElement
                .GetProperty("denormalized")
                .GetProperty(jsonElement.GetProperty("id").GetString()!)
                .GetProperty("data")
                .GetProperty("total_count")
                .GetInt32();
            
            jsonElement = await httpClient.GetJsonAsync(x => {
                x.RequestUri = $"https://api.imvu.com/product?creator={username}&limit={totalCount}".AsUri();
                x.Headers.WithAuthentication(userLogin);
            });
            
            return jsonElement
                .GetProperty("denormalized")
                .EnumerateObject()
                .Where(x =>
                    !x.Name.Equals(jsonElement.GetProperty("id").GetString(),
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
    /// <param name="userLogin"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async ValueTask<RestCreatorModel> GetCreatorInformationAsync(UserLogin userLogin, int userId) {
        if (userId <= 0) {
            throw new ArgumentException("Can't be less than or equal to 0.", nameof(userId));
        }
        
        try {
            var jsonElement = await httpClient.GetJsonAsync(x => {
                x.RequestUri = $"https://api.imvu.com/creator/creator-{userId}".AsUri();
                x.Headers.WithAuthentication(userLogin);
            });
            
            return jsonElement.GetDernormalizedData<RestCreatorModel>();
        }
        catch (Exception exception) {
            logger.LogError("{exception.Message}", exception.Message);
            throw;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="productAction"></param>
    public async ValueTask<IReadOnlyCollection<RestProductModel>> SearchProductsAsync(
        Action<ProductSearchQuery> productAction) {
        var searchQuery = new ProductSearchQuery();
        productAction.Invoke(searchQuery);
        
        var queryBuilder = HttpUtility.ParseQueryString(string.Empty);
        queryBuilder.Add("product_rating", searchQuery.ProductRating == ContentRating.GeneralAudience ? "0" : "1");
        
        if (!string.IsNullOrWhiteSpace(searchQuery.FilterText)) {
            queryBuilder.Add("filter_text", searchQuery.FilterText);
        }
        
        if (!string.IsNullOrWhiteSpace(searchQuery.FilterName)) {
            queryBuilder.Add("name_filter", searchQuery.FilterName);
        }
        
        if (!string.IsNullOrWhiteSpace(searchQuery.PartialAvatarName)) {
            queryBuilder.Add("partial_avatar_name", searchQuery.PartialAvatarName);
        }
        
        if (!string.IsNullOrWhiteSpace(searchQuery.Keywords)) {
            queryBuilder.Add("keywords", searchQuery.Keywords);
        }
        
        if (searchQuery.Gender != null) {
            queryBuilder.Add("gender_restriction", searchQuery.Gender == 'M' ? "male_compatible" : "female_compatible");
        }
        
        queryBuilder.Add("vcoin_price_min", $"{searchQuery.MinimumVCoinPrice}");
        queryBuilder.Add("price_min", $"{searchQuery.MinimumPrice}");
        queryBuilder.Add("include_histogram", $"{searchQuery.IncludeHistogram}");
        
        var jsonElement = await httpClient.GetJsonAsync(x =>
            x.RequestUri = $"https://api.imvu.com/product??{queryBuilder}".AsUri());
        return jsonElement
            .GetProperty("denormalized")
            .EnumerateObject()
            .Select(x => x.Value.GetProperty("data").Deserialize<RestProductModel>())
            .ToArray()!;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="searchQuery"></param>
    /// <returns></returns>
    public ValueTask<IReadOnlyCollection<RestProductModel>> SearchProductsAsync(ProductSearchQuery searchQuery) {
        return SearchProductsAsync(x => {
            x.Keywords = searchQuery.Keywords;
            x.Gender = searchQuery.Gender;
            x.FilterName = searchQuery.FilterName;
            x.FilterText = searchQuery.FilterText;
            x.ProductRating = searchQuery.ProductRating;
        });
        
        // TODO: Complete Mapping
    }
}