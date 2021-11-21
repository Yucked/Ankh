using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ankh.Redis;

/// <summary>
/// 
/// </summary>
public readonly record struct RedisClientOptions {
    /// <summary>
    /// 
    /// </summary>
    public int DatabaseIndex { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public bool TypeNameAsKeyPrefix { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string Hostname { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public JsonSerializerOptions JsonOptions { get; init; }
        = new() {
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.Never,
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
}