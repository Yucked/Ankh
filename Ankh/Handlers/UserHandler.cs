using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
    /// <param name="Username"></param>
    /// <param name="Password"></param>
    /// <param name="SecurityCode"></param>
    private readonly record struct LoginPayload(
        [property: JsonPropertyName("username")]
        string Username,
        [property: JsonPropertyName("password")]
        string Password,
        [property: JsonPropertyName("2fa_code"),
                   JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        string? SecurityCode);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async ValueTask<RestUserModel> GetUserByIdAsync(long userId) {
        if (userId <= 0) {
            throw new ArgumentException("Can't be less than or equal to 0.", nameof(userId));
        }
        
        try {
            var jsonElement = await httpClient.GetJsonAsync(x => {
                x.RequestUri = $"https://api.imvu.com/user/user-{userId}".AsUri();
            });
            
            return jsonElement.GetDernormalizedData<RestUserModel>();
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
    /// <param name="userLogin"></param>
    /// <param name="userIds"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async ValueTask<IReadOnlyList<RestUserModel>> GetUsersByIdAsync(UserLogin userLogin, params int[] userIds) {
        if (userIds.Length == 0) {
            throw new Exception($"{nameof(userIds)} can't be null or empty.");
        }
        
        try {
            var userIdUrls = userIds
                .Select(x => $"https://api.imvu.com/user/user-{x}")
                .ToArray();
            
            var jsonElement = await httpClient.GetJsonAsync(x => {
                x.Headers.WithAuthentication(userLogin);
                x.RequestUri = $"https://api.imvu.com/user?id={string.Join(',', userIdUrls)}".AsUri();
            });
            
            return jsonElement
                .GetProperty("denormalized")
                .EnumerateObject()
                .Select(x => x.Value.GetProperty("data").Deserialize<RestUserModel>())
                .ToArray()!;
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
    public async Task<long> GetIdFromUsernameAsync(string username) {
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
            await httpClient.PostAsync("https://secure.imvu.com//catalog/skudb/gateway.php", data);
        if (!responseMessage.IsSuccessStatusCode) {
            logger.LogError("{ReasonPhrase}", responseMessage.ReasonPhrase);
            return default;
        }
        
        ReadOnlyMemory<byte> byteData = await responseMessage.Content.ReadAsByteArrayAsync();
        var slice = byteData[106..byteData.Span.IndexOf("</int>"u8)];
        return int.Parse(Encoding.UTF8.GetString(slice.Span));
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="mfaCode"></param>
    /// <returns></returns>
    public async Task<UserLogin> LoginAsync(string username, string password, string mfaCode = "") {
        using var responseMessage = await httpClient.PostAsync("https://api.imvu.com/login",
            JsonContent.Create(new LoginPayload {
                Username = username,
                Password = password,
                SecurityCode = string.IsNullOrWhiteSpace(mfaCode) ? null : mfaCode
            }));
        
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
                UserId = int.Parse(userId),
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
            x.RequestUri = $"https://api.imvu.com/user/user-{userLogin.UserId}/outfits?sort=purchased&sort_order=desc"
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