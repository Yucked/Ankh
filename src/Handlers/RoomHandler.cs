using System.Text;
using System.Text.Json;
using Ankh.Data;

namespace Ankh.Handlers;

public class RoomHandler {
    private static readonly ReadOnlyMemory<byte> RoomInfoVar
        = new byte[] {114, 111, 111, 109, 73, 110, 102, 111, 32, 61};

    private static readonly ReadOnlyMemory<byte> ScriptEnd
        = new byte[] {60, 47, 115, 99, 114, 105, 112, 116, 62};

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

        return default;
        /*
        return new RoomData(document.Get<string>("roomInstanceId")) {
            Id = document.GetProperty("roomInstanceId").GetString(),
            Owner = document.GetProperty("owner").GetString(),
            Name = document.GetProperty("room_name").GetString()!.Decode(),
            Description = document.GetProperty("desc").GetString()!.Decode(),
            Capacity = int.Parse(document.GetProperty("max").GetString()!),
            Occupancy = int.Parse(document.GetProperty("count").GetString()!),
            Image = document.GetProperty("img").GetString(),
            IsApOnly = string.IsNullOrWhiteSpace(document.GetProperty("ap").GetString()),
            IsVipOnly = string.IsNullOrWhiteSpace(document.GetProperty("vip").GetString()),
            ApNameOnly = document.GetProperty("show_ap_name_only").GetBoolean(),
            Ratings = document.GetProperty("whitelist_rating").GetInt32(),
            Url = roomUrl.Decode(),
            History = participants,
            AddedOn = DateOnly.FromDateTime(DateTime.Now)
        };
        */
    }

    public static RoomData Update(RoomData before, RoomData after) {
        var updated = before.Update(after);
        foreach (var (user, time) in after.History) {
            updated.History[user] = time;
        }

        return updated;
    }
}