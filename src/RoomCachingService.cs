using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Ankh.Data;

namespace Ankh;

public sealed class RoomCachingService : BackgroundService {
    private readonly HttpClient _httpClient;
    private readonly IBrowsingContext _browsingContext;
    private readonly ILogger<RoomCachingService> _logger;

    private readonly Repository<RoomData> _roomRepository;
    private readonly Repository<DirectoryData> _directoryRepository;

    public RoomCachingService(HttpClient httpClient,
                              IBrowsingContext browsingContext, ILogger<RoomCachingService> logger,
                              Repository<RoomData> roomRepository, Repository<DirectoryData> directoryRepository) {
        _httpClient = httpClient;
        _browsingContext = browsingContext;
        _logger = logger;
        _roomRepository = roomRepository;
        _directoryRepository = directoryRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        _logger.LogInformation($"{nameof(RoomCachingService)} is starting...");
        stoppingToken.Register(() => _logger.LogDebug("RoomCaching task is stopping..."));

        while (!stoppingToken.IsCancellationRequested) {
            await GetOrUpdateRoomDirectoryAsync(stoppingToken);

            var directoryList = await _directoryRepository.GetAllAsync();
            _logger.LogInformation("Total rooms: {Count}", directoryList.Count);

            var roomsDirectory = Random.Shared.HeadsOrTails() == 0
                ? directoryList
                : directoryList.OrderByDescending(x => x.Id).ToList();

            foreach (var directory in roomsDirectory) {
                _logger.LogInformation("Checking {Id} directory", directory.Id);
                foreach (var record in directory.Records) {
                    await ParseRoomAsync(record);
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }

        _logger.LogWarning($"{nameof(RoomCachingService)} has stopped!");
    }

    private async Task GetOrUpdateRoomDirectoryAsync(CancellationToken cancellationToken) {
        using var document = await _browsingContext
            .OpenAsync("https://www.imvu.com/rooms/", cancellationToken);

        var letterList = document.GetElementsByClassName("letter-link notranslate")
            .OfType<IHtmlSpanElement>()
            .Select(x => x.FirstElementChild)
            .OfType<IHtmlAnchorElement>()
            .Select(x => x.Href);
        var roomUrls = new List<string>();

        static bool TryGetNextPage(IDocument document, out string nextPage) {
            nextPage = string.Empty;
            var nextElements = document.All.Where(x =>
                    x.LocalName == "div" &&
                    x.HasAttribute("align") &&
                    x.GetAttribute("align") == "right")
                .ToArray();

            if (nextElements.Length == 0) {
                return false;
            }

            var nextElement = nextElements.FirstOrDefault()?.LastElementChild;
            if (nextElement == null || nextElement.TextContent == "Previous") {
                return false;
            }

            nextPage = ((IHtmlAnchorElement) nextElement).Href;
            return true;
        }

        async Task<string> GetRoomUrlsAsync(string url) {
            using var roomDocument = await _browsingContext.OpenAsync(url, cancellationToken);
            var roomList = roomDocument.GetElementsByClassName("roomdirectory-link notranslate")
                .OfType<IHtmlAnchorElement>()
                .Select(x => x.Href);

            roomUrls.AddRange(roomList);
            return !TryGetNextPage(roomDocument, out var nextPage) ? string.Empty : nextPage;
        }

        foreach (var url in letterList) {
            var nextPage = await GetRoomUrlsAsync(url);
            while (!string.IsNullOrWhiteSpace(nextPage)) {
                nextPage = await GetRoomUrlsAsync(nextPage);
            }

            await _directoryRepository.InsertOrUpdateAsync(new DirectoryData {
                Id = $"{url[^1]}",
                Records = new HashSet<string>(roomUrls)
            });

            roomUrls.Clear();
        }
    }

    private async Task ParseRoomAsync(string url) {
        using var responseMessage = await _httpClient.GetAsync(url);
        if (!responseMessage.IsSuccessStatusCode) {
            _logger.LogWarning(responseMessage.ReasonPhrase);
            return;
        }

        using var content = responseMessage.Content;
        var byteData = await content.ReadAsByteArrayAsync();
        try {
            var room = Extensions.ToRoom(byteData, url);
            await _roomRepository.InsertOrUpdateAsync(room);
        }
        catch (Exception exception) {
            _logger.LogCritical("{Message} {exception}", exception.Message, exception);
        }
    }
}