using Ankh.Api.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var provider = new ServiceCollection()
    .AddLogging(x => x.AddConsole())
    .AddSingleton<HttpClient>()
    .AddSingleton<ProductHandler>()
    .AddSingleton<UserHandler>()
    .AddSingleton<RoomHandler>()
    .BuildServiceProvider();

var productHandler = provider.GetRequiredService<ProductHandler>();
//var product = await productHandler.GetProductByIdAsync();

var userHandler = provider.GetRequiredService<UserHandler>();
//var user = await userHandler.GetUserByIdAsync();
var userId = await userHandler.GetIdFromUsernameAsync("");

//var roomHandler = provider.GetRequiredService<RoomHandler>();
//var room = await roomHandler.GetRoomByIdAsync();

await Task.Delay(-1);