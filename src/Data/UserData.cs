using Ankh.Redis.Interfaces;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable NotAccessedPositionalProperty.Global

namespace Ankh.Data;

/// <summary>
/// 
/// </summary>
/// <param name="Country"></param>
/// <param name="CountryCode"></param>
/// <param name="State"></param>
/// <param name="IsFlagIconVisible"></param>
/// <param name="IsFlagVisible"></param>
public record struct UserLocation(string Country, int CountryCode, object State,
                                  bool IsFlagIconVisible, bool IsFlagVisible);

/// <summary>
/// 
/// </summary>
/// <param name="Url"></param>
/// <param name="IsDefault"></param>
public record struct Avatar(string Url, bool IsDefault);

/// <summary>
/// 
/// </summary>
/// <param name="IsModerator"></param>
/// <param name="WelcomeScore"></param>
public record struct Moderator(bool IsModerator, int WelcomeScore);

/// <summary>
/// 
/// </summary>
/// <param name="IsBuddy"></param>
/// <param name="IsFriend"></param>
/// <param name="IsQualityAssurance"></param>
/// <param name="ShowMessage"></param>
/// <param name="ShowBlock"></param>
public record struct Misc(bool IsBuddy, bool IsFriend, bool IsCreator,
                          bool IsQualityAssurance, bool ShowMessage, bool ShowBlock,
                          int ImvuLevel, int WallpaperId, bool IsAgeVerified);

/// <summary>
/// 
/// </summary>
/// <param name="Status"></param>
/// <param name="Orientation"></param>
/// <param name="LookingFor"></param>
public record struct Dating(string Status, string Orientation, string LookingFor);

/// <summary>
/// 
/// </summary>
/// <param name="Count"></param>
/// <param name="Level"></param>
/// <param name="Layout"></param>
/// <param name="IsCountVisible"></param>
/// <param name="BadgesId"></param>
public record struct Badges(int Count, int Level, string Layout,
                            bool IsCountVisible, IEnumerable<string> BadgesId);

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="AddedOn"></param>
/// <param name="Username"></param>
/// <param name="Homepage"></param>
/// <param name="RegisteredOn"></param>
/// <param name="LastLogon"></param>
/// <param name="Gender"></param>
/// <param name="Age"></param>
/// <param name="Tagline"></param>
/// <param name="Interests"></param>
/// <param name="IsOnline"></param>
/// <param name="Availability"></param>
/// <param name="VisibleAlbums"></param>
/// <param name="Location"></param>
/// <param name="Avatar"></param>
/// <param name="Moderator"></param>
/// <param name="Dating"></param>
/// <param name="Misc"></param>
/// <param name="Badges"></param>
/// <param name="PublicRooms"></param>
/// <param name="Usernames"></param>
public record struct UserData(string Id, DateOnly AddedOn, string Username,
                              string Homepage, DateOnly RegisteredOn, DateOnly LastLogon,
                              string Gender, int Age, string Tagline, string Interests,
                              bool IsOnline, string Availability, int VisibleAlbums,
                              UserLocation Location, Avatar Avatar, Moderator Moderator,
                              Dating Dating, Misc Misc, Badges Badges,
                              IReadOnlyCollection<string> PublicRooms,
                              HashSet<string> Usernames) : IRedisEntity;