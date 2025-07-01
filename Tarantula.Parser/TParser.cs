using System;
using System.Text.RegularExpressions;

namespace Tarantula.Parser
{

    public class TParser
    {
        public static string ExtractTitle(string html)
        {
            Match match = Regex.Match(html, @"<title>\s*(.*?)\s*</title>", RegexOptions.IgnoreCase);
            return match.Success ? match.Groups[1].Value.Trim() : "(no title)";
        }
        public static string ExtractText(string html)
        {
            var imageText = new List<string>();
            var imgMatches = Regex.Matches(html, @"<img[^>]*(alt|title)\s*=\s*[""']([^""']+)[""'][^>]*>", RegexOptions.IgnoreCase);
            foreach (Match match in imgMatches)
            {
                foreach (Capture capture in match.Groups[2].Captures)
                {
                    imageText.Add(capture.Value);
                }
            }

            string noScripts = Regex.Replace(html, @"<script[\s\S]*?</script>", "", RegexOptions.IgnoreCase);
            string noStyles = Regex.Replace(noScripts, @"<style[\s\S]*?</style>", "", RegexOptions.IgnoreCase);

            string noTags = Regex.Replace(noStyles, @"<[^>]+>", " ");
            string cleanText = Regex.Replace(noTags, @"\s+", " ");

            return (string.Join(" ", imageText) + " " + cleanText).Trim();
        }

        public static string ExtractMetaDescription(string html)
        {
            var pattern = @"<meta\s+[^>]*name\s*=\s*[""']description[""'][^>]*content\s*=\s*[""'](.*?)[""'][^>]*>";
            var match = Regex.Match(html, pattern, RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                pattern = @"<meta\s+[^>]*content\s*=\s*[""'](.*?)[""'][^>]*name\s*=\s*[""']description[""'][^>]*>";
                match = Regex.Match(html, pattern, RegexOptions.IgnoreCase);
            }

            return match.Success ? match.Groups[1].Value.Trim() : "(no description)";
        }


    }
}