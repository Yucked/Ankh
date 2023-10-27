#nullable enable
using System.ComponentModel;
using System.Net;
using System.Text.Json;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace Ankh;

public static class Extensions {
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

            if (IsEnumerable(beforeProps[i].PropertyType)) {
                continue;
            }

            if (!IsNull(beforeProp) || IsNull(afterProp)) {
                continue;
            }

            if (IsClassRecordStruct(beforeProps[i].PropertyType)) {
                Update<T>((T) beforeProp!, (T) afterProp!);
            }

            beforeProps[i].SetValue(before, afterProp);
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

        static bool IsClassRecordStruct(Type type) {
            return type.IsClass || type.IsValueType;
        }

        return before;
    }

    public static int HeadsOrTails(this Random random) {
        return random.Next(0, 500) % 2;
    }

    public static string Decode(this string str) {
        return WebUtility.HtmlDecode(WebUtility.UrlDecode(str));
    }

    public static T GetService<T>(this WebApplication application)
        where T : notnull {
        return application.Services.GetRequiredService<T>();
    }

    public static T Get<T>(this JsonElement element, string propertyName,
                           JsonType jsonType = JsonType.None) {
        if (!element.TryGetProperty(propertyName, out var data)) {
            return default!;
        }

        var typeCode = Type.GetTypeCode(typeof(T));
        return typeCode switch {
            TypeCode.String when jsonType == JsonType.Decode
                => (T) (object) WebUtility.HtmlDecode(data.GetString()!),
            TypeCode.String when jsonType == JsonType.IntToStr
                => (T) (object) data.GetInt32().ToString(),
            TypeCode.String
                => (T) (object) data.GetString()!,
            TypeCode.Int32 when jsonType == JsonType.StrToInt
                => (T) (object) (int.TryParse(data.GetString(), out var value) ? value : 0),
            TypeCode.Int32
                => (T) (object) data.GetInt32(),
            TypeCode.Boolean when jsonType == JsonType.IntToBool
                => (T) (object) (data.GetInt32() != 0),
            TypeCode.Boolean
                => (T) (object) data.GetBoolean(),
            _ when jsonType == JsonType.ParseDate
                => (T) (object) DateOnly.Parse(data.GetString()!)
        };
    }

    public static T Get<T>(this JsonProperty property, string propertyName,
                           JsonType jsonType = JsonType.None) {
        return Get<T>(property.Value, propertyName, jsonType);
    }

    public static DateOnly Today() {
        return DateOnly.FromDateTime(DateTime.UtcNow);
    }

    public static async Task<IHtmlDocument> ParseWebPageAsync(this HttpClient httpClient, string url,
                                                              HtmlParser htmlParser,
                                                              CancellationToken cancellationToken) {
        var responseMessage = await httpClient.GetAsync(url, cancellationToken);
        using var content = responseMessage.Content;
        await using var stream = await content.ReadAsStreamAsync(cancellationToken);
        return await htmlParser.ParseDocumentAsync(stream);
    }
}

public enum JsonType {
    None,
    Decode,
    ParseDate,
    IntToBool,
    IntToStr,
    StrToInt
}