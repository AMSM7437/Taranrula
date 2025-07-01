using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Tarantula.Crawler
{
    public class RobotsCache
    {
        private readonly HttpClient httpClient;
        private readonly Dictionary<string, HashSet<string>> disallowedCache = new();

        public RobotsCache(HttpClient client)
        {
            httpClient = client;
        }

        public async Task<bool> IsAllowedAsync(string url)
        {
            var uri = new Uri(url);
            string baseUrl = $"{uri.Scheme}://{uri.Host}";

            if (!disallowedCache.ContainsKey(baseUrl))
                await FetchRobotsTxt(baseUrl);

            string path = uri.PathAndQuery;
            foreach (var disallowed in disallowedCache.GetValueOrDefault(baseUrl, new HashSet<string>()))
            {
                if (path.StartsWith(disallowed))
                    return false;
            }

            return true;
        }

        private async Task FetchRobotsTxt(string baseUrl)
        {
            var disallowed = new HashSet<string>();
            try
            {
                string robotsUrl = $"{baseUrl}/robots.txt";
                string content = await httpClient.GetStringAsync(robotsUrl);

                var lines = content.Split('\n');
                bool userAgentSection = false;

                foreach (string rawLine in lines)
                {
                    string line = rawLine.Trim();
                    if (line.StartsWith("User-agent:"))
                    {
                        userAgentSection = line.Contains("*");
                    }
                    else if (userAgentSection && line.StartsWith("Disallow:"))
                    {
                        string path = line.Substring(9).Trim();
                        if (!string.IsNullOrWhiteSpace(path))
                            disallowed.Add(path);
                    }
                }
            }
            catch
            {
               
            }

            disallowedCache[baseUrl] = disallowed;
        }
    }
}
