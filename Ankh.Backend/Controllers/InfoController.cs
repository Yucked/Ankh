using Ankh.Handlers;
using Ankh.Models.Rest;
using Ankh.Models.Rework;
using Microsoft.AspNetCore.Mvc;

namespace Ankh.Backend.Controllers;

[ApiController, Route("api/[controller]"), Produces("application/json")]
public sealed class InfoController(
    Database database,
    UserHandler userHandler,
    RoomHandler roomHandler) : ControllerBase {
    [HttpGet("user")]
    public async ValueTask<IActionResult> GetUserInformationAsync(string userId) {
        if (string.IsNullOrWhiteSpace(userId)) {
            return BadRequest($"Missing {nameof(userId)} in request.");
        }
        
        var user = await database.GetByIdAsync<RestUserModel>(userId);
        if (!string.IsNullOrWhiteSpace(user.Username)) {
            return Ok(user);
        }
        
        user = await userHandler.GetUserByIdAsync(int.Parse(userId));
        await database.SaveAsync(user);
        return Ok(user);
    }
    
    [HttpPost("room")]
    public async ValueTask<IActionResult> GetRoomInformationAsync(string roomId) {
        if (string.IsNullOrWhiteSpace(roomId)) {
            return BadRequest($"Missing {nameof(roomId)} in request.");
        }
        
        var room = await database.GetByIdAsync<RoomModel>(roomId);
        if (!string.IsNullOrWhiteSpace(room.Name)) {
            return Ok(room);
        }
        
        room = await roomHandler.GetRoomByIdAsync(roomId, Database.RandomLogin);
        await database.SaveAsync(room);
        return Ok(room);
    }
}