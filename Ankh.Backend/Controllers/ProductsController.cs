using Ankh.Api.Models;
using Ankh.Handlers;
using Ankh.Models.Rest;
using Ankh.Models.Rework;
using Microsoft.AspNetCore.Mvc;

namespace Ankh.Backend.Controllers;

public readonly record struct Response(string Query);

[ApiController, Route("api/[controller]"), Produces("application/json")]
public sealed class ProductsController(
    ProductHandler productHandler,
    RoomHandler roomHandler,
    UserHandler userHandler,
    Database database) : ControllerBase {
    [HttpPost("scene")]
    public async Task<IActionResult> GetSceneProductsAsync(Response response) {
        if (string.IsNullOrWhiteSpace($"{response.Query}")) {
            return BadRequest($"Missing {nameof(response.Query)} in request.");
        }
        
        var sceneProducts = await productHandler.GetProductsInSceneAsync($"{response.Query}");
        var result = new Dictionary<long, object>();
        
        foreach (var (key, value) in sceneProducts) {
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
   
    
    [HttpGet("ripper")]
    public ValueTask<IActionResult> RipProductAsync(Response response) {
        return ValueTask.FromResult<IActionResult>(Ok());
    }
}