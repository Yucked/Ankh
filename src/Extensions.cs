#nullable enable
using System.ComponentModel;
using System.Net;

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

        return before;
    }

    public static int HeadsOrTails(this Random random) {
        return random.Next(0, 500) % 2;
    }

    public static string Decode(this string str) {
        return WebUtility.HtmlDecode(WebUtility.UrlDecode(str));
    }

    public static string Id(this string str, string id) {
        return str.Replace("{USER_ID}", id);
    }

    public static string Id(this string str, int id) {
        return str.Replace("{USER_ID}", $"{id}");
    }

    public static T GetService<T>(this WebApplication application) {
#pragma warning disable CS8714
        return application.Services.GetRequiredService<T>();
#pragma warning restore CS8714
    }
}