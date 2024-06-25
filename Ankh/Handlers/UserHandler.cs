using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Ankh.Models.Rest;
using Ankh.Models.Rework;
using Microsoft.Extensions.Logging;

namespace Ankh.Handlers;

public sealed class UserHandler(
    ILogger<UserHandler> logger,
    HttpClient httpClient) {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async ValueTask<UserModel> GetUserByIdAsync(string userId) {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        
        try {
            var restJson = await httpClient.GetJsonAsync(x => {
                x.RequestUri = $"https://api.imvu.com/user/user-{userId}".AsUri();
            });
            
            var phpJson = await httpClient.GetJsonAsync(x => {
                x.RequestUri = $"https://client-dynamic.imvu.com/api/avatarcard.php?cid={userId}".AsUri();
            });
            
            return Extensions
                .MergeJson(restJson, phpJson)
                .Deserialize<UserModel>()!;
        }
        catch (Exception exception) {
            logger.LogError("{exception.Message}", exception.Message);
            throw;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async ValueTask<RestProfileModel> GetUserProfileAsync(long userId) {
        if (userId <= 0) {
            throw new ArgumentException("Can't be less than or equal to 0.", nameof(userId));
        }
        
        try {
            var jsonElement = await httpClient.GetJsonAsync(x => {
                x.RequestUri = $"https://api.imvu.com/profile/profile-user-{userId}".AsUri();
            });
            
            return jsonElement.GetDernormalizedData<RestProfileModel>();
        }
        catch (Exception exception) {
            logger.LogError("{exception.Message}", exception.Message);
            throw;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userIds"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async ValueTask<IReadOnlyList<UserModel>> GetUsersByIdAsync(params string[] userIds) {
        if (userIds.Length == 0) {
            throw new Exception($"{nameof(userIds)} can't be null or empty.");
        }
        
        try {
            return await Task.WhenAll(userIds.Select(GetUserByIdAsync).Select(x => x.AsTask()));
        }
        catch (Exception exception) {
            logger.LogError("{exception.Message}", exception.Message);
            throw;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public async Task<string> GetIdFromUsernameAsync(string username) {
        using var data = new StringContent(
            $"""
                         <methodCall>
                         <methodName>gateway.getUserIdForAvatarName</methodName>
                         <params>
                             <param>
                                 <value>
                                     <string>{username}</string>
                                 </value>
                             </param>
                         </params>s
                         </methodCall>
             """,
            Encoding.UTF8, "application/xml");
        
        using var responseMessage =
            await httpClient.PostAsync("https://secure.imvu.com/catalog/skudb/gateway.php", data);
        if (!responseMessage.IsSuccessStatusCode) {
            logger.LogError("{ReasonPhrase}", responseMessage.ReasonPhrase);
            return string.Empty;
        }
        
        ReadOnlyMemory<byte> byteData = await responseMessage.Content.ReadAsByteArrayAsync();
        var slice = byteData[106..byteData.Span.IndexOf("</int>"u8)];
        return Encoding.UTF8.GetString(slice.Span);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="mfaCode"></param>
    /// <returns></returns>
    public async Task<UserLogin> LoginAsync(string username, string password, string mfaCode = "") {
        var dict = new Dictionary<string, string>() {
            { "username", username },
            { "password", password }
        };
        
        if (!string.IsNullOrWhiteSpace(mfaCode)) {
            dict.Add("2fa_code", mfaCode);
        }
        
        using var responseMessage = await httpClient.PostAsync("https://api.imvu.com/login", JsonContent.Create(dict));
        using var document = await JsonDocument.ParseAsync(await responseMessage.Content.ReadAsStreamAsync());
        if (!document.RootElement.TryGetProperty("error", out var errorElement)) {
            var data = document
                .RootElement
                .GetProperty("denormalized")
                .GetProperty(document.RootElement.GetProperty("id").GetString()!)
                .GetProperty("data");
            
            var loginId = document
                .RootElement
                .GetProperty("id")
                .GetString()!
                .Split('/')[^1];
            
            var sauce = data
                .GetProperty("sauce")
                .GetString()!;
            
            var userId = data
                .GetProperty("user")
                .GetProperty("id")
                .GetString()!
                .Split('/')[^1];
            
            return new UserLogin {
                Username = username,
                Password = password,
                Sauce = sauce,
                Id = userId,
                SessionId = loginId
            };
        }
        
        var errorCode = errorElement.GetString();
        var errorMessage = errorCode == "LOGIN-017"
            ? $"Call {nameof(LoginAsync)} again with 2FA code."
            : document.RootElement.GetProperty("message").GetString();
        
        logger.LogError("{errorCode}: {errorMessage}", errorCode, errorMessage);
        return default!;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userLogin"></param>
    /// <returns></returns>
    public async Task<RestOutfitModel?[]> GetUserOutfitsAsync(UserLogin userLogin) {
        var jsonElement = await httpClient.GetJsonAsync(x => {
            x.Headers.WithAuthentication(userLogin);
            x.RequestUri = $"https://api.imvu.com/user/user-{userLogin.Id}/outfits?sort=purchased&sort_order=desc"
                .AsUri();
        });
        
        return jsonElement
            .GetProperty("denormalized")
            .EnumerateObject()
            .Where(x => x.Name.Contains("api.imvu.com/outfit/outfit"))
            .Select(x => x.Value.GetProperty("data").Deserialize<RestOutfitModel>())
            .ToArray();
    }
}