using AngleSharp;
using Ankh;
using Ankh.Caching;
using Microsoft.Extensions.Logging.Colorful;
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
    .AddSingleton<UserCacher>()
    .AddSingleton<RoomCacher>()
    .AddSingleton<DirectoryCacher>()
    //.AddHostedService<CachingService>()
    .AddSingleton(BrowsingContext.New(Configuration.Default.WithDefaultLoader()))
    .AddSingleton(ConnectionMultiplexer.Connect(builder.GetConnection("dedis")));


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