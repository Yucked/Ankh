using System.Collections.Concurrent;

namespace Ankh.Api;

public readonly record struct UserSauce(
    string Username,
    string UserId,
    string Sauce,
    string Auth);

public readonly record struct Saucery {
    private static readonly ConcurrentDictionary<string, UserSauce> Sauces = new();
    
    public static bool TryStore(string username, UserSauce sauce) {
        return Sauces.TryAdd(username, sauce);
    }
    
    public static bool TryRemove(string username) {
        return Sauces.TryRemove(username, out _);
    }
    
    public static bool TryGet(string username, out UserSauce sauce) {
        return Sauces.TryGetValue(username, out sauce!);
    }
}