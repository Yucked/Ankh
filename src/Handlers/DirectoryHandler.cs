using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Ankh.Data;

namespace Ankh.Handlers;

public sealed class DirectoryHandler {
    private readonly IBrowsingContext _browsingContext;
    private readonly HttpClient _httpClient;
    private readonly HtmlParser _htmlParser;
    private readonly ILogger<DirectoryHandler> _logger;

    public DirectoryHandler(ILogger<DirectoryHandler> logger,
                            HttpClient httpClient,
                            HtmlParser htmlParser,
                            IBrowsingContext browsingContext) {
        _logger = logger;
        _httpClient = httpClient;
        _htmlParser = htmlParser;
        _browsingContext = browsingContext;
    }

    public static DirectoryData Update(DirectoryData before, DirectoryData after) {
        before.Records.UnionWith(after.Records);
        before.Count = before.Records.Count;
        return before;
    }

    public async Task CacheDirectoriesAsync(CancellationToken cancellationToken) {
        using var document = await _httpClient.ParseWebPageAsync(Endpoints.ROOMS, _htmlParser, cancellationToken);
        var letterList = document.GetElementsByClassName("letter-link notranslate")
            .Select(x => ((x as IHtmlSpanElement).FirstElementChild as IHtmlAnchorElement).Href);

        var roomUrls = new List<string>();
        foreach (var url in letterList) {
            var nextPage = await GetRoomUrlsAsync(url, cancellationToken);
            while (!string.IsNullOrWhiteSpace(nextPage)) {
                nextPage = await GetRoomUrlsAsync(nextPage, cancellationToken);
            }

            var data = new DirectoryData {
                Id = $"{url[^1]}",
                Records = new HashSet<string>(roomUrls),
                Count = roomUrls.Count,
            };

            AddToCache(data.Id, data);
            roomUrls.Clear();
        }
    }

    private async Task<string> GetRoomUrlsAsync(string url, CancellationToken cancellationToken) {
        using var document = await _httpClient.ParseWebPageAsync(url, _htmlParser, cancellationToken);
        var roomList = document.GetElementsByClassName("roomdirectory-link notranslate")
            .Select(x => (x as IHtmlAnchorElement).Href);

        roomUrls.AddRange(roomList);
        return !TryGetNextPage(document, out var nextPage) ? string.Empty : nextPage;
    }

    private static bool TryGetNextPage(IDocument document, out string nextPage) {
        nextPage = string.Empty;
        var nextElements = document.All
            .Where(x =>
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

    private async IAsyncEnumerable<string> ParseUrlsAsync(string url, CancellationToken cancellationToken) {
        using var document = await _httpClient.ParseWebPageAsync(url, _htmlParser, cancellationToken);
        var roomList = document.GetElementsByClassName("roomdirectory-link notranslate")
            .Select(x => (x as IHtmlAnchorElement).Href);

        var nextElements = document.All
            .Where(x =>
                x.LocalName == "div" &&
                x.HasAttribute("align") &&
                x.GetAttribute("align") == "right")
            .ToArray();

        if (nextElements.Length == 0) {
            yield return roomList;
        }

        var nextElement = nextElements.FirstOrDefault()?.LastElementChild;
        if (nextElement == null || nextElement.TextContent == "Previous") {
            yield return roomList;
        }

        var nextPage = ((IHtmlAnchorElement) nextElement).Href;
    }
}