using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Revisions;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace Ankh.Backend.Workers;

public sealed class StartupWorker(
    IDocumentStore documentStore,
    ILogger<StartupWorker> logger) : BackgroundService {
    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        try {
            await documentStore
                .Maintenance
                .Server
                .SendAsync(new CreateDatabaseOperation(new DatabaseRecord(nameof(Ankh))), stoppingToken);
            
            await documentStore
                .Maintenance
                .SendAsync(new ConfigureRevisionsOperation(new RevisionsConfiguration {
                    Default = new RevisionsCollectionConfiguration {
                        Disabled = false
                    }
                }), stoppingToken);
        }
        catch {
            logger.LogWarning("Raven operations already in place.");
        }
    }
}