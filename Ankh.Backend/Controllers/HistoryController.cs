using Ankh.Models.Rework;
using Microsoft.AspNetCore.Mvc;

namespace Ankh.Backend.Controllers;


[ApiController, Route("api/[controller]"), Produces("application/json")]
public sealed class HistoryController(Database database) : ControllerBase{
    [HttpGet("username")]
    public async ValueTask<IActionResult> GetUsernameHistoryAsync(string userId) {
        if (string.IsNullOrWhiteSpace(userId)) {
            return BadRequest($"Missing {nameof(userId)} in request.");
        }
        
        var revisions = await database.GetRevisionsAsync<UserModel>(userId);
        var usernames = revisions.Select(x => x.Username).Distinct();
        return Ok(usernames);
    }
    
    [HttpGet("rooms")]
    public async ValueTask<IActionResult> GetUserRoomHistoryAsync(string userId) {
        if (string.IsNullOrWhiteSpace(userId)) {
            return BadRequest($"Missing {nameof(userId)} in request.");
        }
        
        return Ok();
    }
}