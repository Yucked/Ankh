using Ankh.Backend;
using Ankh.Handlers;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Playwright;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Revisions;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

var exitCode = Microsoft.Playwright.Program.Main(["install", "--with-deps", "chromium"]);
if (exitCode != 0) {
    throw new Exception($"Playwright exited with code {exitCode}");
}

var playwright = await Playwright.CreateAsync();
var browser = await playwright.Chromium.LaunchAsync();

var builder = WebApplication.CreateSlimBuilder();
builder.Services.AddControllers();
builder.Services
    .AddSingleton<Database>()
    .AddSingleton<UserHandler>()
    .AddSingleton<RoomHandler>()
    .AddSingleton<ProductHandler>()
    .AddSingleton(browser)
    .AddHttpClient()
    .AddLogging(x => {
        x.ClearProviders();
        x.AddSimpleConsole(f => {
            f.ColorBehavior = LoggerColorBehavior.Enabled;
            f.UseUtcTimestamp = true;
            f.IncludeScopes = true;
            f.SingleLine = false;
            f.TimestampFormat = "yy/MM/dd HH:mm:ss ";
        });
    })
    .AddSingleton<IDocumentStore>(x => new DocumentStore {
        Urls = x.GetService<IConfiguration>()!.GetValue<string[]>("RavenNodes"),
        Conventions = {
            CreateHttpClient = f => x.GetService<IHttpClientFactory>()!.CreateClient("RavenDB"),
            UseOptimisticConcurrency = true,
            MaxNumberOfRequestsPerSession = 30,
            RequestTimeout = TimeSpan.FromSeconds(2)
        },
        Database = nameof(Ankh)
    }.Initialize())
    .AddHealthChecks();

var app = builder.Build();
app.UseHttpsRedirection();
app.MapControllers();
app.UseHealthChecks("/health");

await await app
    .RunAsync()
    .ContinueWith(_ => EnableRavenFeaturesAsync());
return;

async Task EnableRavenFeaturesAsync() {
    var store = app.Services.GetRequiredService<IDocumentStore>();
    try {
        await store.Maintenance.Server.SendAsync(new CreateDatabaseOperation(new DatabaseRecord(nameof(Ankh))));
        await store.Maintenance.SendAsync(new ConfigureRevisionsOperation(new RevisionsConfiguration {
            Default = new RevisionsCollectionConfiguration {
                Disabled = false
            }
        }));
    }
    catch {
        app.Services.GetService<ILogger<Program>>()!.LogWarning("Raven operations already in place.");
    }
}