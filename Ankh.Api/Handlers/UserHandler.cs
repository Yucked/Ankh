using System.Text.Json;
using Ankh.Api.Models;
using Microsoft.Extensions.Logging;

namespace Ankh.Api.Handlers;

public sealed class UserHandler(
    ILogger<UserHandler> logger,
    HttpClient httpClient) {
    public ValueTask<UserModel> GetUserByIdAsync(long userId) {
        if (userId <= 0) {
            throw new ArgumentException("Can't be less than or equal to 0.", nameof(userId));
        }
        
        try {
            return httpClient.GetRestModelAsync<UserModel>($"https://api.imvu.com/user/user-{userId}");
        }
        catch (Exception exception) {
            logger.LogError(exception, "Something went wrong.");
            throw;
        }
    }
    
    // TODO: Requires Cookies or make individual requests
    public async ValueTask<IReadOnlyList<UserModel>> GetUsersByIdAsync(params int[] userIds) {
        if (userIds.Length == 0) {
            throw new Exception("");
        }
        
        try {
            var userIdUrls = userIds
                .Select(x => $"https://api.imvu.com/user/user-{x}")
                .ToArray();
            
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get,
                    $"https://api.imvu.com/user?id={string.Join(',', userIdUrls)}")
                .AddLoginCookie();
            
            var responseMessage = await httpClient.SendAsync(requestMessage);
            if (!responseMessage.IsSuccessStatusCode) {
                throw new Exception($"Failed to fetch because of {responseMessage.ReasonPhrase}");
            }
            
            await using var stream = await responseMessage.Content.ReadAsStreamAsync();
            using var document = await JsonDocument.ParseAsync(stream);
            var status = document.RootElement.GetProperty("status").GetString();
            
            var denorm = document.RootElement.GetProperty("denormalized");
            return userIdUrls
                .Select(x => denorm.GetProperty(x).GetProperty("data").Deserialize<UserModel>())
                .ToList()!;
        }
        catch (Exception exception) {
            logger.LogError(exception, "Something went wrong.");
            throw;
        }
    }
}