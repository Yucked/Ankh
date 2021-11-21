namespace Ankh.Redis;

/// <summary>
/// 
/// </summary>
public readonly struct RedisClientConnection {
    /// <summary>
    /// 
    /// </summary>
    public string Hostname { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string Password { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public int DatabaseIndex { get; init; }

    internal string Connection
        => $"{Hostname},password={Password},defaultDatabase={DatabaseIndex}";
}