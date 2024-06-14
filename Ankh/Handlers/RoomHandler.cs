using System.Text.Json;
using System.Web;
using Ankh.Models.Enums;
using Ankh.Models.Queries;
using Ankh.Models.Rework;
using Microsoft.Extensions.Logging;

namespace Ankh.Handlers;

public sealed class RoomHandler(
    ILogger<RoomHandler> logger,
    HttpClient httpClient) {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="userSauce"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async ValueTask<RoomModel> GetRoomByIdAsync(string roomId, UserSauce userSauce) {
        ArgumentException.ThrowIfNullOrWhiteSpace(roomId);
        
        try {
            var restJson = await httpClient.GetJsonAsync(x => {
                x.RequestUri = $"https://api.imvu.com/room/room-{roomId}".AsUri();
            });
            
            var phpJson = await httpClient.GetJsonAsync(x => {
                x.RequestUri = $"https://client-dynamic.imvu.com/api/rooms/room_info.php?room_id={roomId}".AsUri();
                x.Headers.WithAuthentication(userSauce);
            });
            
            return Extensions
                .MergeJson(restJson, phpJson)
                .Deserialize<RoomModel>()!;
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
    public async ValueTask<IReadOnlyList<RoomModel>> SearchRoomsAsync(UserSauce userSauce,
                                                                      Action<RoomSearchQuery> searchQuery) {
        var query = new RoomSearchQuery();
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
        
        var jsonElement = await httpClient
            .GetJsonAsync(x => {
                x.RequestUri = $"https://api.imvu.com/user/user-{userSauce.UserId}/filtered_rooms?{queryBuilder}"
                    .AsUri();
                x.Headers.WithAuthentication(userSauce);
            });
        
        return jsonElement
            .GetProperty("denormalized")
            .EnumerateObject()
            .Select(x => jsonElement.GetProperty("denormalized").GetProperty(x.Name).GetProperty("data")
                .Deserialize<RoomModel>())
            .ToArray()!;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userSauce"></param>
    /// <param name="userIds"></param>
    /// <returns></returns>
    public async ValueTask<IDictionary<long, RoomModelMinimal[]>> GetPublicRoomsForUsersAsync(
        UserSauce userSauce,
        params long[] userIds) {
        var jsonElement =
            await httpClient.GetJsonAsync(x => {
                x.RequestUri =
                    $"https://client-dynamic.imvu.com/api/find_locations.php?cids={string.Join(',', userIds)}".AsUri();
                x.Headers.WithAuthentication(userSauce);
            });
        
        var results = jsonElement.GetProperty("result");
        return (results.ValueKind == JsonValueKind.Array
            ? null
            : results
                .EnumerateObject()
                .Select(x => {
                    var rooms = x.Value
                        .EnumerateArray()
                        .Select(y => y.Deserialize<RoomModelMinimal>())
                        .ToArray();
                    return (long.Parse(x.Name), rooms);
                })
                .ToDictionary(x => x.Item1, y => y.rooms))!;
    }
}