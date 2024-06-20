using System.Text.Json.Serialization;

namespace Ankh.Models.Rework;

public record UserModelEssential {
    [JsonPropertyName("id")]
    public string Id { get; init; }
    public string Username { get; init; }
}

public record UserLogin : UserModelEssential {
    public string Password { get; init; }
    public string Sauce { get; init; }
    public string SessionId { get; init; }
}

public record UserModelMinimal : UserModelEssential {
    
}

public record UserModelCommon : UserModelMinimal {
    public string ProfileImage { get; init; }
    
    [JsonPropertyName("registered")]
    public DateTimeOffset RegisteredOn { get; init; }
    
    [JsonPropertyName("gender")]
    public string Gender { get; init; }
    
    [JsonPropertyName("age")]
    public int Age { get; init; }
}

public record UserModel : UserModelCommon { }