using System.Text;
using System.Text.Json;
using Ankh.Api.Models;
using Ankh.Api.Models.Enums;
using Microsoft.Extensions.Logging;

namespace Ankh.Api.Handlers;

public class RoomHandler(
    ILogger<RoomHandler> logger,
    HttpClient httpClient) {
    public ValueTask<RoomModel> GetRoomByIdAsync(long userId, int roomId) {
        if (userId <= 0 || roomId <= 0) {
            throw new ArgumentException("Can't be less than or equal to 0.", nameof(userId));
        }
        
        try {
            return httpClient.GetRestModelAsync<RoomModel>($"https://api.imvu.com/room/room-{userId}-{roomId}");
        }
        catch (Exception exception) {
            logger.LogError(exception, "Something went wrong.");
            throw;
        }
    }
    
    // TODO: Requires cookies
    public async ValueTask<IReadOnlyList<RoomModel>> SearchRoomsAsync(string keywords,
                                                                      string language = "en",
                                                                      string avatarName = "",
                                                                      int minOccpuants = 1,
                                                                      int maxOccupants = 10,
                                                                      bool hasPlusProducts = false,
                                                                      bool requiresAccessPass = false,
                                                                      ContentRating rating =
                                                                          ContentRating.GeneralAudience) {
        // TODO: RandomUserId doesn't work for this API call. Requires logged in user's cookie.
        var stringBuilder =
            new StringBuilder($"https://api.imvu.com/user/user-{Extensions.GetRandomUserId()}filtered_rooms?");
        stringBuilder.Append($"language={language}&");
        stringBuilder.Append($"low={minOccpuants}&");
        stringBuilder.Append($"high={maxOccupants}&");
        stringBuilder.Append("supports_audience=0&");
        stringBuilder.Append($"plus_filter={hasPlusProducts}&");
        switch (requiresAccessPass) {
            case true when rating is ContentRating.AccessPass:
                stringBuilder.Append("ap=1&rating=ap&has_access_pass=1");
                break;
            case false when rating is ContentRating.GeneralAudience:
                stringBuilder.Append("ap=0&rating=ga&ga_only=1");
                break;
        }
        
        stringBuilder.Append(
            $"filter_text={keywords}&name_filter={avatarName}&partial_avatar_name={avatarName}&keywords={keywords}");
        
        using var responseMessage = await httpClient.GetAsync($"{stringBuilder}");
        if (!responseMessage.IsSuccessStatusCode) {
            throw new Exception($"Failed to fetch because of {responseMessage.ReasonPhrase}");
        }
        
        await using var stream = await responseMessage.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);
        
        // TODO: status returns success even if child items return 403 need to work on this.
        var status = document.RootElement.GetProperty("status").GetString();
        
        var denorm = document.RootElement.GetProperty("denormalized");
        return denorm
            .EnumerateObject()
            .Select(x => denorm.GetProperty(x.Name).GetProperty("data").Deserialize<RoomModel>())
            .ToArray()!;
    }
}