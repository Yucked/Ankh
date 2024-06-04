using System.Text.Json;
using System.Web;
using Ankh.Api.Models.Enums;
using Ankh.Api.Models.Interfaces;
using Ankh.Api.Models.Rest;
using Microsoft.Extensions.Logging;

namespace Ankh.Api.Handlers;

public class RoomHandler(
    ILogger<RoomHandler> logger,
    HttpClient httpClient) {
    /// <summary>
    /// 
    /// </summary>
    public sealed record SearchQuery {
        /// <summary>
        /// 
        /// </summary>
        public string Keywords { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Language { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Username { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int? MinOccupants { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int? MaxOccupants { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool? HasPlusProducts { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool? RequiresAccessPass { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public ContentRating? Rating { get; set; }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roomId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public ValueTask<RestRoomModel> GetRoomByIdAsync(long userId, int roomId) {
        if (userId <= 0 || roomId <= 0) {
            throw new ArgumentException("Can't be less than or equal to 0.", nameof(userId));
        }
        
        try {
            return httpClient.GetRestModelAsync<RestRoomModel>($"https://api.imvu.com/room/room-{userId}-{roomId}");
        }
        catch (Exception exception) {
            logger.LogError(exception, "Something went wrong.");
            throw;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userSauce"></param>
    /// <param name="searchQuery"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async ValueTask<IReadOnlyList<RestRoomModel>> SearchRoomsAsync(
        UserSauce userSauce, Action<SearchQuery> searchQuery) {
        userSauce.VerifyLogin();
        
        var query = new SearchQuery();
        searchQuery.Invoke(query);
        
        var queryBuilder = HttpUtility.ParseQueryString(string.Empty);
        if (!string.IsNullOrWhiteSpace(query.Language)) {
            queryBuilder.Add("language", query.Language);
        }
        
        if (query.MinOccupants != null) {
            queryBuilder.Add("low", $"{query.MinOccupants}");
        }
        
        if (query.MaxOccupants != null) {
            queryBuilder.Add("high", $"{query.MaxOccupants}");
        }
        
        queryBuilder.Add("supports_audience", "0");
        if (query.HasPlusProducts != null) {
            queryBuilder.Add("plus_filter", $"{query.HasPlusProducts}");
        }
        
        switch (query.RequiresAccessPass) {
            case true when query.Rating is ContentRating.AccessPass:
                queryBuilder.Add("ap", "1");
                queryBuilder.Add("rating", "ap");
                queryBuilder.Add("has_access_pass", "1");
                break;
            case false when query.Rating is ContentRating.GeneralAudience:
                queryBuilder.Add("ap", "0");
                queryBuilder.Add("rating", "ga");
                queryBuilder.Add("ga_only", "1");
                break;
        }
        
        if (!string.IsNullOrWhiteSpace(query.Keywords)) {
            queryBuilder.Add("filter_text", query.Keywords);
        }
        
        if (!string.IsNullOrWhiteSpace(query.Username)) {
            queryBuilder.Add("name_filter", query.Username);
        }
        
        if (!string.IsNullOrWhiteSpace(query.Username)) {
            queryBuilder.Add("partial_avatar_name", query.Username);
        }
        
        if (!string.IsNullOrWhiteSpace(query.Keywords)) {
            queryBuilder.Add("keywords", query.Keywords);
        }
        
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get,
                $"https://api.imvu.com/user/user-{userSauce.UserId}/filtered_rooms?{queryBuilder}")
            .WithCookieSauce(userSauce.Auth);
        
        var responseMessage = await httpClient.SendAsync(requestMessage);
        await using var stream = await responseMessage.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);
        
        Extensions.IsRequestSuccessful(responseMessage.StatusCode, document.RootElement);
        
        return document
            .RootElement
            .GetProperty("denormalized")
            .EnumerateObject()
            .Select(x => document.RootElement.GetProperty("denormalized").GetProperty(x.Name).GetProperty("data")
                .Deserialize<RestRoomModel>())
            .ToArray()!;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userIds"></param>
    /// <returns></returns>
    public async ValueTask<IDictionary<long, VURoomModel[]>> GetPublicRoomsForUsersAsync(params long[] userIds) {
        var responseMessage =
            await httpClient.GetAsync($"https://client-dynamic.imvu.com/api/find_locations.php?cids={userIds}");
        
        if (!responseMessage.IsSuccessStatusCode) {
            logger.LogError("{responseMessage.StatusCode}: {responseMessage.ReasonPhrase}",
                responseMessage.StatusCode,
                responseMessage.ReasonPhrase);
            throw new Exception(responseMessage.ReasonPhrase);
        }
        
        using var document = await JsonDocument.ParseAsync(await responseMessage.Content.ReadAsStreamAsync());
        return document
            .RootElement
            .GetProperty("result")
            .EnumerateObject()
            .Select(x => {
                var rooms = x.Value
                    .EnumerateArray()
                    .Select(y => y.Deserialize<VURoomModel>())
                    .ToArray();
                return (long.Parse(x.Name), rooms);
            })
            .ToDictionary(x => x.Item1, y => y.rooms)!;
    }
}