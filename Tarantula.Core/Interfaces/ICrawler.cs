using Tarantula.Core.Classes;

namespace Tarantula.Core.Interfaces

{
    public interface ICrawler
    {
        //CrawlState State { get; }
        //Task StartAsync(string startUrl);
        //Task Stop();
        //Task Pause();
        //Task Resume();
        //event Action<CrawlState> StateChanged;
        event Action<PageError> PageErrored;

    }
}
