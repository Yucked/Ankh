namespace Ankh;

public readonly record struct Endpoints {
    /// <summary>
    /// 
    /// </summary>
    public const string BASE_URL = "https://www.imvu.com";

    /// <summary>
    /// 
    /// </summary>
    public const string ROOMS = $"{BASE_URL}/rooms";

    /// <summary>
    /// 
    /// </summary>
    public const string API = "https://api.imvu.com";

    /// <summary>
    /// 
    /// </summary>
    public const string GATEWAY_PHP = "http://secure.imvu.com//catalog/skudb/gateway.php";

    /// <summary>
    /// 
    /// </summary>
    public const string AVATAR_CARD = $"{API}/avatarcard.php?cid={{USER_ID}}";

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

    /// <summary>
    /// 
    /// </summary>
    public const string BLOCKLIST = $"{API}/blocklist/{{USER_ID}}";

    /// <summary>
    /// 
    /// </summary>
    public const string AVATAR = $"{API}/avatar/avatar-{{USER_ID}}";

    /// <summary>
    /// 
    /// </summary>
    public const string DEFAULT_ROOM = $"{API}/room/room-{{USER_ID}}-466";

    /// <summary>
    /// 
    /// </summary>
    public const string SUBSCRIBED_FEED = $"{API}/feed/feed-subscribed-{{USER_ID}}";

    /// <summary>
    /// 
    /// </summary>
    public const string RECOMMENDED_FEED = $"{API}/feed/feed-recommended-adult";

    /// <summary>
    /// 
    /// </summary>
    public const string WALLET = $"{API}/wallet/wallet-{{USER_ID}}";

    /// <summary>
    /// 
    /// </summary>
    public const string PHOTO_BOOTH_ROOM = $"{API}/room/room-{{USER_ID}}-392";

    /// <summary>
    /// 
    /// </summary>
    public const string CART = $"{API}/cart/cart-{{USER_ID}}";

    /// <summary>
    /// 
    /// </summary>
    public const string ROOM_MANAGEMENT_INFO = $"{API}/room_management_info/room_management_info-{{USER_ID}}";

    /// <summary>
    /// 
    /// </summary>
    public const string TENURE = $"{API}/tenure/tenure-{{USER_ID}}";

    /// <summary>
    /// 
    /// </summary>
    public const string LEANPLUM_DATA = $"{API}/leanplum_data/leanplum_data-{{USER_ID}}";

    /// <summary>
    /// 
    /// </summary>
    public const string USER_PERMISSIONS = $"{API}/user_permissions/user_permissions-{{USER_ID}}";

    /// <summary>
    /// 
    /// </summary>
    public const string APPSFLYER_EVENT_TRACKING_URL = $"{API}/appsflyer_data/appsflyer_data-{{USER_ID}}";

    /// <summary>
    /// 
    /// </summary>
    public const string RECOMMENDED_ROOMS = $"{API}/user/user-{{USER_ID}}/recommended_rooms";

    /// <summary>
    /// 
    /// </summary>
    public const string SECURITY_SETTINGS = $"{API}/security_settings/security_settings-{{USER_ID}}";

    /// <summary>
    /// 
    /// </summary>
    public const string ROULETTE = $"{API}/roulette/roulette-{{USER_ID}}";

    /// <summary>
    /// 
    /// </summary>
    public const string CREATOR = $"{API}/earner/user-{{USER_ID}}";

    /// <summary>
    /// 
    /// </summary>
    public const string FRIENDS = $"{API}/user/user-{{USER_ID}}/friends";

    /// <summary>
    /// 
    /// </summary>
    public const string EMAIL_PREFERENCES = $"{API}/user/user-{{USER_ID}}/email_preferences";

    /// <summary>
    /// 
    /// </summary>
    public const string INVITES = $"{API}/user/user-{{USER_ID}}/invites";

    /// <summary>
    /// 
    /// </summary>
    public const string INBOUND_FRIEND_REQUESTS = $"{API}/user/user-{{USER_ID}}/inbound_friend_requests";

    /// <summary>
    /// 
    /// </summary>
    public const string OUTBOUND_FRIEND_REQUESTS = $"{API}/user/user-{{USER_ID}}/outbound_friend_requests";

    /// <summary>
    /// 
    /// </summary>
    public const string OUTFITS = $"{API}/user/user-{{USER_ID}}/outfits";

    /// <summary>
    /// 
    /// </summary>
    public const string FAVORITE_ROOMS = $"{API}/user/user-{{USER_ID}}/favorite_rooms";

    /// <summary>
    /// 
    /// </summary>
    public const string RECENT_ROOMS = $"{API}/user/user-{{USER_ID}}/recent_rooms";

    /// <summary>
    /// 
    /// </summary>
    public const string FILTERED_ROOMS = $"{API}/user/user-{{USER_ID}}/filtered_rooms";

    /// <summary>
    /// 
    /// </summary>
    public const string RECENTLY_TRIED_PRODUCTS = $"{API}/user/user-{{USER_ID}}/recently_tried_products";

    /// <summary>
    /// 
    /// </summary>
    public const string PREFERENCES = $"{API}/user/user-{{USER_ID}}/preferences";

    /// <summary>
    /// 
    /// </summary>
    public const string WELCOME_ROOMS = $"{API}/user/user-{{USER_ID}}/welcome_rooms";

    /// <summary>
    /// 
    /// </summary>
    public const string REWARDS = $"{API}/user/user-{{USER_ID}}/rewards";

    /// <summary>
    /// 
    /// </summary>
    public const string FILTERED_USERS = $"{API}/user/user-{{USER_ID}}/filtered_users";

    /// <summary>
    /// 
    /// </summary>
    public const string MUSIC_FAVORITES = $"{API}/user/user-{{USER_ID}}/music_favorites";

    /// <summary>
    /// 
    /// </summary>
    public const string INVENTORY = $"{API}/user/user-{{USER_ID}}/inventory";

    /// <summary>
    /// 
    /// </summary>
    public const string ALBUMS = $"{API}/user/user-{{USER_ID}}/albums";

    /// <summary>
    /// 
    /// </summary>
    public const string UML_USERS = $"{API}/user/user-{{USER_ID}}/uml_users";

    /// <summary>
    /// 
    /// </summary>
    public const string ACTIVITY = $"{API}/user/user-{{USER_ID}}/activity";

    /// <summary>
    /// 
    /// </summary>
    public const string PURCHASES = $"{API}/user/user-{{USER_ID}}/purchases";

    /// <summary>
    /// 
    /// </summary>
    public const string BLOCKED = $"{API}/user/user-{{USER_ID}}/blocked";

    /// <summary>
    /// 
    /// </summary>
    public const string CONVERSATIONS = $"{API}/user/user-{{USER_ID}}/conversations";

    /// <summary>
    /// 
    /// </summary>
    public const string GIFTLIST = $"{API}/user/user-{{USER_ID}}/giftlist";

    /// <summary>
    /// 
    /// </summary>
    public const string ALLOWED_APPS = $"{API}/user/user-{{USER_ID}}/allowed_apps";

    /// <summary>
    /// 
    /// </summary>
    public const string APPS = $"{API}/user/user-{{USER_ID}}/apps";

    /// <summary>
    /// 
    /// </summary>
    public const string PLAYERS = $"{API}/user/user-{{USER_ID}}/players";

    /// <summary>
    /// 
    /// </summary>
    public const string ACCOUNT_ORDERS = $"{API}/user/user-{{USER_ID}}/account_orders";

    /// <summary>
    /// 
    /// </summary>
    public const string MANAGED_ROOMS = $"{API}/user/user-{{USER_ID}}/managed_rooms";

    /// <summary>
    /// 
    /// </summary>
    public const string VIP_SUBSCRIPTIONS = $"{API}/user/user-{{USER_ID}}/vip_subscriptions";

    /// <summary>
    /// 
    /// </summary>
    public const string SUBSCRIPTIONS = $"{API}/user/user-{{USER_ID}}/subscriptions";

    /// <summary>
    /// 
    /// </summary>
    public const string SCENES = $"{API}/user/user-{{USER_ID}}/scenes";

    /// <summary>
    /// 
    /// </summary>
    public const string MY_ROOMS = $"{API}/user/user-{{USER_ID}}/my_rooms";

    /// <summary>
    /// 
    /// </summary>
    public const string MANAGED_EVENTS = $"{API}/user/user-{{USER_ID}}/managed_events";

    /// <summary>
    /// 
    /// </summary>
    public const string NOTIFICATION_TYPES = $"{API}/user/user-{{USER_ID}}/notification_types";

    /// <summary>
    /// 
    /// </summary>
    public const string MY_RESPONDED_EVENTS = $"{API}/user/user-{{USER_ID}}/my_responded_events";

    /// <summary>
    /// 
    /// </summary>
    public const string PRODUCT_PACKAGES_CART = $"{API}/user/user-{{USER_ID}}/product_packages_cart";

    /// <summary>
    /// 
    /// </summary>
    public const string INBOUND_TIPS = $"{API}/user/user-{{USER_ID}}/inbound_tips";

    /// <summary>
    /// 
    /// </summary>
    public const string OUTBOUND_TIPS = $"{API}/user/user-{{USER_ID}}/outbound_tips";

    /// <summary>
    /// 
    /// </summary>
    public const string VCOIN_TRANSACTIONS = $"{API}/user/user-{{USER_ID}}/vcoin_transactions";

    /// <summary>
    /// 
    /// </summary>
    public const string GIFTS = $"{API}/user/user-{{USER_ID}}/gifts";

    /// <summary>
    /// 
    /// </summary>
    public const string BROADCAST_SETTINGS = $"{API}/broadcast_settings/broadcast_settings-{{USER_ID}}";
}