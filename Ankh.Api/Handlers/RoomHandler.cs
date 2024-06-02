﻿using System.Text.Json;
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
    /// <param name="Keywords"></param>
    /// <param name="Language"></param>
    /// <param name="AvatarName"></param>
    /// <param name="MinOccpuants"></param>
    /// <param name="MaxOccupants"></param>
    /// <param name="HasPlusProducts"></param>
    /// <param name="RequiresAccessPass"></param>
    /// <param name="Rating"></param>
    public record struct SearchQuery(
        string Keywords,
        string Language = "en",
        string AvatarName = "",
        int MinOccpuants = 1,
        int MaxOccupants = 10,
        bool HasPlusProducts = false,
        bool RequiresAccessPass = false,
        ContentRating Rating = ContentRating.GeneralAudience);
    
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
        queryBuilder.Add("language", query.Language);
        queryBuilder.Add("low", $"{query.MinOccpuants}");
        queryBuilder.Add("high", $"{query.MaxOccupants}");
        queryBuilder.Add("supports_audience", "0");
        queryBuilder.Add("plus_filter", $"{query.HasPlusProducts}");
        
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
        
        queryBuilder.Add("filter_text", query.Keywords);
        queryBuilder.Add("name_filter", query.AvatarName);
        queryBuilder.Add("partial_avatar_name", query.AvatarName);
        queryBuilder.Add("keywords", query.Keywords);
        
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get,
                $"https://api.imvu.com/user/user-{userSauce.UserId}/filtered_rooms?{queryBuilder}")
            .WithCookieSauce(userSauce.Auth);
        
        var responseMessage = await httpClient.SendAsync(requestMessage);
        await using var stream = await responseMessage.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);
        
        Extensions.IsRequestSuccessful(responseMessage.StatusCode, document.RootElement);
        
        var denorm = document.RootElement.GetProperty("denormalized");
        return denorm
            .EnumerateObject()
            .Select(x => denorm.GetProperty(x.Name).GetProperty("data").Deserialize<RestRoomModel>())
            .ToArray()!;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async ValueTask<VURoomModel> GetPublicRoomsForUserAsync(long userId) {
        // https://client-dynamic.imvu.com/api/find_locations.php?cids={userIds}
        return default;
    }
}