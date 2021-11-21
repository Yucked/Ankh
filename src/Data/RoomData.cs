using Ankh.Redis.Interfaces;

namespace Ankh.Data;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="AddedOn"></param>
/// <param name="Name"></param>
/// <param name="IsApOnly"></param>
/// <param name="IsVipOnly"></param>
public record PublicRoomData(string Id, DateOnly AddedOn, string Name,
                             bool IsApOnly, bool IsVipOnly) : IRedisEntity;

/// <summary>
/// 
/// </summary>
/// <param name="Owner"></param>
/// <param name="Description"></param>
/// <param name="Capacity"></param>
/// <param name="Occupancy"></param>
/// <param name="Image"></param>
/// <param name="Ratings"></param>
/// <param name="Url"></param>
/// <param name="ApNameOnly"></param>
/// <param name="History"></param>
public record RoomData(string Id, DateOnly AddedOn, string Name,
                       bool IsApOnly, bool IsVipOnly,
                       string Owner, string Description, int Capacity,
                       int Occupancy, string Image, int Ratings, string Url,
                       bool ApNameOnly, IDictionary<string, DateTimeOffset> History)
    : PublicRoomData(Id, AddedOn, Name, IsApOnly, IsApOnly);