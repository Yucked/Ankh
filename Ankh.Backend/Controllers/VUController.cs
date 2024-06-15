using Ankh.Api.Models;
using Ankh.Handlers;
using Ankh.Models.Rest;
using Ankh.Models.Rework;
using Microsoft.AspNetCore.Mvc;

namespace Ankh.Backend.Controllers;

public readonly record struct Response(string Query);

[ApiController, Route("api/[controller]"), Produces("application/json")]
public sealed class VUController(
    ProductHandler productHandler,
    RoomHandler roomHandler,
    UserHandler userHandler,
    Database database) : ControllerBase {
    [HttpPost("scene-products")]
    public async Task<IActionResult> GetSceneProductsAsync(Response response) {
        if (string.IsNullOrWhiteSpace($"{response.Query}")) {
            return BadRequest($"Missing {nameof(response.Query)} in request.");
        }
        
        var scenceProducts = await productHandler.GetProductsInSceneAsync($"{response.Query}");
        var result = new Dictionary<long, object>();
        
        foreach (var (key, value) in scenceProducts) {
            var user = await database.GetByIdAsync<RestUserModel>(key);
            var products = new List<RestProductModel>();
            
            if (!string.IsNullOrWhiteSpace(user.Username)) {
                user = await userHandler.GetUserByIdAsync(user.UserId);
                await database.SaveAsync(user);
            }
            
            await Parallel.ForEachAsync(value, async (v, _) => {
                var product = await database.GetByIdAsync<RestProductModel>(v);
                if (!string.IsNullOrWhiteSpace(product.ProductName)) {
                    products.Add(product);
                    return;
                }
                
                product = await productHandler.GetProductByIdAsync(int.Parse(v));
                await database.SaveAsync(product);
                products.Add(product);
            });
            
            result.Add(user.UserId, new {
                user,
                products
            });
        }
        
        return Ok(result);
    }
    
    [HttpPost("user-info")]
    public async ValueTask<IActionResult> GetUserInformationAsync(Response response) {
        if (string.IsNullOrWhiteSpace($"{response.Query}")) {
            return BadRequest($"Missing {nameof(response.Query)} in request.");
        }
        
        var user = await database.GetByIdAsync<RestUserModel>(response.Query);
        if (!string.IsNullOrWhiteSpace(user.Username)) {
            return Ok(user);
        }
        
        user = await userHandler.GetUserByIdAsync(int.Parse(response.Query));
        await database.SaveAsync(user);
        return Ok(user);
    }
    
    [HttpPost("room-info")]
    public async ValueTask<IActionResult> GetRoomInformationAsync(Response response) {
        if (string.IsNullOrWhiteSpace($"{response.Query}")) {
            return BadRequest($"Missing {nameof(response.Query)} in request.");
        }
        
        var room = await database.GetByIdAsync<RoomModel>(response.Query);
        if (!string.IsNullOrWhiteSpace(room.Name)) {
            return Ok(room);
        }
        
        room = await roomHandler.GetRoomByIdAsync(response.Query, Database.RandomLogin);
        await database.SaveAsync(room);
        return Ok(room);
    }
    
    [HttpPost("history-username")]
    public ValueTask<IActionResult> GetUsernameHistoryAsync(Response response) {
        return ValueTask.FromResult<IActionResult>(Ok());
    }
    
    [HttpPost("history-user-rooms")]
    public ValueTask<IActionResult> GetUserRoomHistoryAsync(Response response) {
        return ValueTask.FromResult<IActionResult>(Ok());
    }
    
    [HttpGet("rip-product")]
    public ValueTask<IActionResult> RipProductAsync(Response response) {
        return ValueTask.FromResult<IActionResult>(Ok());
    }
}