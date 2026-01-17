namespace Tarantula.Core.Classes
{
    public class PageResult
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Url { get; set; }
        public string Html { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public string? Meta { get; set; }
        public PageResult(Guid Id, string url, string html)
        {
            this.Id = Id;
            this.Url = url;
            this.Html = html;
        }
    }
}
