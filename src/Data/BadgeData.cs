using Ankh.Redis.Interfaces;

namespace Ankh.Data;

/// <summary>
/// 
/// </summary>
/// <param name="Width"></param>
/// <param name="Height"></param>
public record struct Dimensions(int Width, int Height);

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="FlaggedOn"></param>
public record struct Flag(string Id, DateTime FlaggedOn);

/// <summary>
/// 
/// </summary>
/// <param name="X"></param>
/// <param name="Y"></param>
public record struct Coordinates(int X, int Y);

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Index"></param>
public record struct Creator(string Id, int Index);

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="IsAutogranted"></param>
/// <param name="Type"></param>
/// <param name="ReviewStatus"></param>
/// <param name="Url"></param>
/// <param name="Creator"></param>
/// <param name="Flag"></param>
/// <param name="AddedOn"></param>
/// <param name="Dimensions"></param>
/// <param name="Coordinates"></param>
public record struct BadgeData(string Id, string Name, bool IsAutogranted,
                               string Type, string ReviewStatus, string Url,
                               Creator Creator, Flag Flag, DateOnly AddedOn,
                               Dimensions Dimensions, Coordinates Coordinates) : IRedisEntity;