using Ankh.Api.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Ankh.Tests;

public static class Globals {
    private static IServiceProvider Provider
        => new ServiceCollection()
            .AddLogging()
            .AddSingleton<HttpClient>()
            .AddSingleton<ProductHandler>()
            .AddSingleton<UserHandler>()
            .AddSingleton<RoomHandler>()
            .BuildServiceProvider();
    
    public static UserHandler UserHandler
        => Provider.GetRequiredService<UserHandler>();
    
    public static RoomHandler RoomHandler
        => Provider.GetRequiredService<RoomHandler>();
}