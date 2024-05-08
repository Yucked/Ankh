using System.Net.Http.Json;
using Ankh.Api.Models;
using Microsoft.Extensions.Logging;

namespace Ankh.Api.Handlers;

public sealed class UserHandler(
    ILogger<UserHandler> logger,
    HttpClient httpClient) {
    public async ValueTask<UserModel> GetUserByIdAsync(long userId) {
        if (userId <= 0) {
            throw new ArgumentException("Can't be less than or equal to 0.", nameof(userId));
        }
        
        var apiUrl = $"https://api.imvu.com/user/user-{userId}";
        using var responseMessage = await httpClient.GetAsync(apiUrl);
        if (!responseMessage.IsSuccessStatusCode) {
            logger.LogError("Failed product fetch {}", userId);
            throw new Exception("");
        }
        
        var restModel = await responseMessage.Content.ReadFromJsonAsync<RestModel>();
        if (restModel!.Status != "success") {
            throw new Exception("");
        }
        
        return (UserModel)restModel.Data;
    }
}