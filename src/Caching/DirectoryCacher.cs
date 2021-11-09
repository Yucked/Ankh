using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Ankh.Data;

namespace Ankh.Caching;

public record DirectoryCacher : AbstractCacher<DirectoryData> {
    private readonly IBrowsingContext _browsingContext;
    private readonly List<string> _roomUrls;

    public DirectoryCacher(ILogger<DirectoryData> logger, HttpClient httpClient,
        IBrowsingContext browsingContext)
        : base(logger, httpClient) {
        _browsingContext = browsingContext;
        _roomUrls = new List<string>();
    }

    public async Task CacheDirectoriesAsync(CancellationToken cancellationToken) {
        using var document = await _browsingContext
            .OpenAsync(Endpoints.ROOMS, cancellationToken);

        var letterList = document.GetElementsByClassName("letter-link notranslate")
            .Select(x => (x as IHtmlSpanElement).FirstElementChild as IHtmlAnchorElement)
            .Select(x => x.Href);

        foreach (var url in letterList) {
            var nextPage = await GetRoomUrlsAsync(url, cancellationToken);
            while (!string.IsNullOrWhiteSpace(nextPage)) {
                nextPage = await GetRoomUrlsAsync(nextPage, cancellationToken);
            }

            AddToCache(new DirectoryData {
                Id = $"{url[^1]}",
                Records = new HashSet<string>(_roomUrls)
            });

            _roomUrls.Clear();
        }
    }

    private static bool TryGetNextPage(IDocument document, out string nextPage) {
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

        nextPage = ((IHtmlAnchorElement)nextElement).Href;
        return true;
    }

    private async Task<string> GetRoomUrlsAsync(string url, CancellationToken cancellationToken) {
        using var roomDocument = await _browsingContext.OpenAsync(url, cancellationToken);
        var roomList = roomDocument.GetElementsByClassName("roomdirectory-link notranslate")
            .OfType<IHtmlAnchorElement>()
            .Select(x => x.Href);

        _roomUrls.AddRange(roomList);
        return !TryGetNextPage(roomDocument, out var nextPage) ? string.Empty : nextPage;
    }
}
