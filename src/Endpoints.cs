namespace Ankh;

public readonly record struct Endpoints {
    /// <summary>
    /// 
    /// </summary>
    public const string API = "https://api.imvu.com";

    /// <summary>
    /// 
    /// </summary>
    public const string OUTFIT = $"{API}/profile_outfit/profile_outfit-{{USER_ID}}";

    /// <summary>
    /// 
    /// </summary>
    public const string WHISTLIST = $"{API}/user/user-{{USER_ID}}/wishlist";

    /// <summary>
    /// 
    /// </summary>
    public const string PERSONAL_FEED = $"{API}/feed/feed-personal-{{USER_ID}}";

    /// <summary>
    /// 
    /// </summary>
    public const string PROFILE = $"{API}/profile/profile-user-{{USER_ID}}";
    
    /// <summary>
    /// 
    /// </summary>
    public const string PRESENCE = $"{API}/presence/presence-{{USER_ID}}";
}