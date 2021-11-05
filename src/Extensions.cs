#nullable enable
using Ankh.Data;
using System.ComponentModel;
using System.Collections;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Ankh;

public static class Extensions {
    private static readonly ReadOnlyMemory<byte> RoomInfoVar
        = new byte[] {114, 111, 111, 109, 73, 110, 102, 111, 32, 61};

    private static readonly ReadOnlyMemory<byte> ScriptEnd
        = new byte[] {60, 47, 115, 99, 114, 105, 112, 116, 62};

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

    public static RoomData ToRoom(Span<byte> data, string roomUrl) {
        var slice = data[(data.IndexOf(RoomInfoVar.Span) + RoomInfoVar.Length)..];
        slice = slice[..slice.IndexOf(ScriptEnd.Span)];

        var json = Encoding.UTF8
            .GetString(slice)
            .Replace("'", "\"");

        var document = JsonDocument.Parse(json).RootElement;
        var participants = new Dictionary<string, DateTimeOffset>();
        foreach (var name in document.GetProperty("participants")
                     .EnumerateArray()
                     .Select(participant => participant.GetProperty("name").GetString())) {
            participants[name!] = DateTimeOffset.UtcNow;
        }

        return new RoomData {
            Id = document.GetProperty("roomInstanceId").GetString(),
            Owner = document.GetProperty("owner").GetString(),
            Name = document.GetProperty("room_name").GetString(),
            Description = WebUtility.UrlDecode(document.GetProperty("desc").GetString()),
            Capacity = int.Parse(document.GetProperty("max").GetString()!),
            Occupancy = int.Parse(document.GetProperty("count").GetString()!),
            Image = document.GetProperty("img").GetString(),
            IsApOnly = string.IsNullOrWhiteSpace(document.GetProperty("ap").GetString()),
            IsVipOnly = string.IsNullOrWhiteSpace(document.GetProperty("vip").GetString()),
            ApNameOnly = document.GetProperty("show_ap_name_only").GetBoolean(),
            Ratings = document.GetProperty("whitelist_rating").GetInt32(),
            Url = roomUrl,
            UserHistory = participants
        };
    }
}