using System;
using System.Threading.Tasks;
using Tarantula.Crawler;
using Tarantula.Indexer;
using Tarantula.Models;
using Tarantula.Parser;

class TRunner
{
    static async Task Main(string[] args)
    {
        var crawler = new TCrawler( maxPages : 500);
        var dbPath = Path.Combine(Environment.CurrentDirectory, "index.db");
        var indexer = new TIndexer();


        await foreach (var page in crawler.CrawlStreamAsync("http://localhost"))
        {
            page.Title = TParser.ExtractTitle(page.Html);
            page.Text = TParser.ExtractText(page.Html);
            page.Meta = TParser.ExtractMetaDescription(page.Html);

            indexer.AddPage(page);
            Console.WriteLine($"\n=== {page.Url} ===");
            Console.WriteLine($"\nTitle: {page.Title}");
            Console.WriteLine($"\nContent Preview: {page.Text}");
            Console.WriteLine($"\nMeta: {page.Meta}");
        }
    }
}
