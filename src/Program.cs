using AngleSharp;
using Ankh;
using Microsoft.Extensions.Logging.Colorful;
using ServiceStack.Redis;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services
    .AddHttpClient()
    .AddLogging((Microsoft.Extensions.Logging.ILoggingBuilder x) => {
        x.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
        x.ClearProviders();
        x.AddColorfulConsole();
    })
    .AddSingleton<Database>()
    .AddHostedService<RoomCachingService>()
    .AddSingleton(BrowsingContext.New(Configuration.Default.WithDefaultLoader()))
    .AddSingleton<IRedisClientsManagerAsync>(
    new BasicRedisClientManager(builder.Configuration.GetConnectionString("Redis")));


LoggingExtensions.ChangeConsoleMode();
var app = builder.Build();
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

await app.RunAsync();