#nullable enable
using System.Collections;
using System.ComponentModel;
using System.Net;
using System.Text;
using Ankh.Data;

namespace Ankh;

public static class Extensions {
    private static readonly ReadOnlyMemory<byte> EndSegment
        = Encoding.UTF8.GetBytes("</int>");

    public static T Update<T>(this T before, T after) {
        ArgumentNullException.ThrowIfNull(before, nameof(before));
        ArgumentNullException.ThrowIfNull(after, nameof(after));

        var beforeProps = TypeDescriptor.GetProperties(before);
        var afterProps = TypeDescriptor.GetProperties(after);
        for (var i = 0; i < beforeProps.Count; i++) {
            var beforeProp = beforeProps[i].GetValue(before);
            var afterProp = afterProps[i].GetValue(after);

            if (IsNull(beforeProp) && IsNull(afterProp)) {
                continue;
            }

            if (IsEqual(beforeProp, afterProp)) {
                continue;
            }

            if (!IsEnumerable(beforeProps[i].PropertyType)) {
                beforeProps[i].SetValue(before, afterProp);
                continue;
            }

            if (IsNull(beforeProp) && !IsNull(afterProp)) {
                beforeProps[i].SetValue(before, afterProp);
                continue;
            }

            var collection = ((IEnumerable) beforeProp!).Cast<object>()
                .Concat(((IEnumerable) afterProp!).Cast<object>())
                .Distinct();

            //beforeProps[i].SetValue(before, collection);
        }

        static bool IsEnumerable(Type type) {
            return type.IsGenericType && type.Namespace == "System.Collections.Generic";
        }

        static bool IsNull(object? obj) {
            return obj == null || obj.Equals(null) || obj.Equals(default);
        }

        static bool IsEqual(object? obj, object? val) {
            return !IsNull(obj) == !IsNull(val) && obj == val;
        }

        return before;
    }

    public static int HeadsOrTails(this Random random) {
        return random.Next(0, 500) % 2;
    }

    public static async Task<UserData> GetUserAsync(this HttpClient httpClient, int id) {
        using var responseMessage = await httpClient.GetAsync($"http://www.imvu.com/api/avatarcard.php?cid={id}");
        if (!responseMessage.IsSuccessStatusCode) {
            throw new Exception(responseMessage.ReasonPhrase);
        }

        using var content = responseMessage.Content;
        await using var stream = await content.ReadAsStreamAsync();
        return await UserData.BuildUserAsync(stream);
    }

    public static async Task<int> GetIdAsync(this HttpClient httpClient, string username) {
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

        using var responseMessage =
            await httpClient.PostAsync("http://secure.imvu.com//catalog/skudb/gateway.php", data);
        if (!responseMessage.IsSuccessStatusCode) {
            throw new Exception(responseMessage.ReasonPhrase);
        }

        using var content = responseMessage.Content;
        ReadOnlyMemory<byte> byteData = await content.ReadAsByteArrayAsync();
        var strId = Encoding.UTF8.GetString(byteData[106..byteData.Span.IndexOf(EndSegment.Span)].Span);
        return int.Parse(strId);
    }

    public static string Decode(this string str) {
        return WebUtility.HtmlDecode(WebUtility.UrlDecode(str));
    }
}