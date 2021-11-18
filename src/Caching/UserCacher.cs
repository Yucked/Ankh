using Ankh.Data;
using System.Text;

namespace Ankh.Caching;

public sealed record UserCacher
    : AbstractCacher<UserData> {
    private static readonly ReadOnlyMemory<byte> EndSegment = new byte[] {
        60, 47, 105, 110, 116, 62
    };

    public UserCacher(ILogger<UserData> logger, HttpClient httpClient)
        : base(logger, httpClient) { }

    public async Task CacheUserAsync(int id) {
        using var responseMessage = await HttpClient.GetAsync(Endpoints.AVATAR_CARD.Id(id));
        if (!responseMessage.IsSuccessStatusCode) {
            Logger.LogError("{ReasonPhrase}", responseMessage.ReasonPhrase);
            return;
        }

        using var content = responseMessage.Content;
        await using var stream = await content.ReadAsStreamAsync();
        var user = await UserData.BuildUserAsync(stream);
        AddToCache(user.Id, user);
    }

    public async Task<int> GetIdAsync(string username) {
        using var data = new StringContent(
            @$"
        <methodCall>
        <methodName>gateway.getUserIdForAvatarName</methodName>
        <params>
            <param>
                <value>
                    <string>{username}</string>
                </value>
            </param>
        </params>s
        </methodCall>",
            Encoding.UTF8, "application/xml");

        using var responseMessage = await HttpClient.PostAsync(Endpoints.GATEWAY_PHP, data);
        if (!responseMessage.IsSuccessStatusCode) {
            Logger.LogError("{ReasonPhrase}", responseMessage.ReasonPhrase);
            return default;
        }

        using var content = responseMessage.Content;
        ReadOnlyMemory<byte> byteData = await content.ReadAsByteArrayAsync();
        var slice = byteData[106..byteData.Span.IndexOf(EndSegment.Span)];
        return int.Parse(Encoding.UTF8.GetString(slice.Span));
    }
}