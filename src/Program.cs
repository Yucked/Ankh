using AngleSharp;
using Ankh;
using Ankh.Data;
using Marten;
using Marten.Services;
using Microsoft.Extensions.Logging.Colorful;
using Weasel.Core;
using Weasel.Postgresql;

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
    .AddSingleton<Repository<UserData>>()
    .AddSingleton<Repository<RoomData>>()
    .AddSingleton<Repository<DirectoryData>>()
    .AddHostedService<RoomCachingService>()
    .AddSingleton(BrowsingContext.New(Configuration.Default.WithDefaultLoader()))
    .AddMarten(x => {
        x.Connection(builder.Configuration.GetConnectionString("Postgres"));
        x.AutoCreateSchemaObjects = AutoCreate.All;
        x.CreateDatabasesForTenants(c => c.ForTenant(nameof(Ankh)));
        x.Serializer(new SystemTextJsonSerializer {
            Casing = Casing.SnakeCase,
            EnumStorage = EnumStorage.AsString
        });
    });

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

app.Run();