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
}