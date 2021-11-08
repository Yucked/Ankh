using AngleSharp;
using Ankh;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Colorful;

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
    .AddSingleton<IBrowsingContext>(BrowsingContext.New(Configuration.Default.WithDefaultLoader()))
    .AddDbContext<IMVUContext>(x => x.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

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

await app.EnsureDbCreationAsync();

app.Run();