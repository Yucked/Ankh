using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace Ankh;

public sealed class Spyder(
    ILogger<Spyder> logger,
    IBrowser browser) {
    private static readonly string[] BlockedResources = [
        "adzerk",
        "analytics",
        "cdn.api.twitter",
        "doubleclick",
        "exelator",
        "facebook",
        "fontawesome",
        "google",
        "google-analytics",
        "googletagmanager",
        "googlesyndication",
        "disqus",
        "ads",
        "analytic"
    ];
    
    public async Task<IPage?> RequestPageAsync(string url) {
        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();
        
        await page.RouteAsync("**/*", async route => {
            if (BlockedResources.Any(y => route.Request.Url.Contains(y))) {
                logger.LogDebug("Route matched with blocked resources, aborted: {}", route.Request.Url);
                await route.AbortAsync();
                return;
            }
            
            await route.ContinueAsync();
        });
        
        var response = await page.GotoAsync(url);
        if (response?.Ok is not true) {
            logger.LogError("Playwright couldn't fetch {}", url);
            return null;
        }
        
        await response.FinishedAsync();
        return page;
    }
}