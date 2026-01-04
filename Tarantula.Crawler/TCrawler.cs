using System.Diagnostics;
using System.Text.RegularExpressions;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Buffers.Text;
using System.Timers;
using System.Data.SqlTypes;
using System.Diagnostics.Contracts;
using Tarantula.Models;


namespace Tarantula.Crawler;

public class TCrawler
{
    private readonly HttpClient httpClient;
    private readonly Queue<string> urlQueue = new Queue<string>();
    private readonly HashSet<string> visitedUrls = new HashSet<string>();
    private readonly int maxPages;
    private int foundLinks;
    private readonly RobotsCache robots;

    public TCrawler(int maxPages)
    {
        this.maxPages = maxPages;
        httpClient = new HttpClient();
        //httpClient.Timeout = TimeSpan.FromSeconds(30);
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
        robots = new RobotsCache(httpClient);


    }
    public async IAsyncEnumerable<PageResult> CrawlStreamAsync(string url)
    {
        urlQueue.Enqueue(url);
        Stopwatch sw = Stopwatch.StartNew();

        while (urlQueue.Count > 0 && visitedUrls.Count < maxPages)
        {
            string currentUrl = urlQueue.Dequeue();
            if (visitedUrls.Contains(currentUrl)) continue;

            Console.WriteLine($"\nCrawling ({visitedUrls.Count + 1}/{maxPages}): {currentUrl}");
            visitedUrls.Add(currentUrl);

            //if (!await robots.IsAllowedAsync(currentUrl))
            //{
            //    Console.WriteLine($"Blocked by robots.txt: {currentUrl}");
            //    continue;
            //}

            string html = await DownloadPageAsync(currentUrl);
            if (html == null)
            {
                Console.WriteLine($"Failed to download or skipped non-html content: {currentUrl}");
                continue;
            }

            Guid id = Guid.NewGuid();
            var page = new PageResult(id, currentUrl, html);
            yield return page; // emit page immediately

            List<string> links = ExtractLinks(html, currentUrl);
            Console.WriteLine($" => Found {links.Count} links");
            foundLinks += links.Count;

            foreach (var link in links)
            {
                if (!visitedUrls.Contains(link) && !urlQueue.Contains(link))
                    urlQueue.Enqueue(link);
            }
        }

        sw.Stop();
        Console.WriteLine($"\nTarantula crawled {visitedUrls.Count} pages and found {foundLinks} links in {sw.ElapsedMilliseconds / 1000} seconds\n.");
    }

    private async Task<string> DownloadPageAsync(string url)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            if (response != null && response.Content.Headers.ContentType?.MediaType != null && !response.Content.Headers.ContentType.MediaType.Contains("text/html"))
            {
                return null;
            }

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading page: {ex.Message}");
            return null;
        }
    }


    private List<string> ExtractLinks(string html, string url)
    {
        var links = new List<string>();
        string pattern = @"href\s*=\s*[""'](?<url>[^""'#>]+)[""']";
        foreach (Match match in Regex.Matches(html, pattern))
        {
            string href = match.Groups["url"].Value;
            Uri baseUri = new Uri(url);

            if (Uri.TryCreate(baseUri, href, out Uri? fullUri))
            {
                links.Add(fullUri.AbsoluteUri);
            }
        }
        return links;
    }
}