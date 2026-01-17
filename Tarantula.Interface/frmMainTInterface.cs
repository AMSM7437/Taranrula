using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Tarantula.Core.Classes;
using Tarantula.Crawler;
using Tarantula.Indexer;
using Tarantula.Parser;

namespace Tarantula.Interface
{
   
    public partial class frmMainTInterface : Form
    {
        TCrawler crawler = new TCrawler(maxPages: 500);
        TIndexer indexer = new TIndexer();
        public  frmMainTInterface()
        {
            InitializeComponent();
            _=InitializeRunner(); // only temporary
        }
        public static void HandlePageError(PageError pageError)
        {
            Console.WriteLine(pageError.errMsg);
        }
        public async Task InitializeRunner()
        {
            var crawler = new TCrawler(maxPages: 500);
            crawler.PageErrored += HandlePageError;

            var indexer = new TIndexer();


            await foreach (var page in crawler.CrawlStreamAsync("http://localhost"))
            {
                page.Title = TParser.ExtractTitle(page.Html);
                page.Text = TParser.ExtractText(page.Html);
                page.Meta = TParser.ExtractMetaDescription(page.Html);

                indexer.AddPage(page);
                //Console.WriteLine($"\n=== {page.Url} ===");
                //Console.WriteLine($"\nTitle: {page.Title}");
                //Console.WriteLine($"\nContent Preview: {page.Text}");
                //Console.WriteLine($"\nMeta: {page.Meta}");

                dgvCrawledUrls.Rows.Add(page.Url,page.Title,page.Meta);
            }
        }

    }
}
