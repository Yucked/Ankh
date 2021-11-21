using AngleSharp;
using Ankh.Caching;
using Ankh.Redis;
using Microsoft.Extensions.Logging.Colorful;

var builder = WebApplication.CreateBuilder();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services
    .AddHttpClient()
    .AddLogging(x => {
        x.SetMinimumLevel(LogLevel.Information);
        x.ClearProviders();
        x.AddColorfulConsole();
    })
    .AddSingleton<UserCacher>()
    .AddSingleton<RoomCacher>()
    .AddSingleton<DirectoryCacher>()
    .AddHostedService<CachingService>()
    .AddSingleton<RedisClientManager>()
    .AddSingleton(BrowsingContext.New(Configuration.Default.WithDefaultLoader()));

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