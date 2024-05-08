using Ankh.Api.Models;
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
}