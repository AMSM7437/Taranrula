using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Tarantula.Models;


namespace Tarantula.Indexer
{
    public class TIndexer
    {
        private readonly Dictionary<string, HashSet<string>> index = new();
        public void addPage(PageResult page)
        {
            string[] tokens = Tokenize(page.Text);
            foreach (string token in tokens) {
                if (!index.ContainsKey(token)) { 
                        index[token] = new HashSet<string>();
                }
                index[token].Add(page.Url);
            }

        }
        public  async Task<List<string>> Search(string word)
        {

            word = word.ToLowerInvariant();
            return index.ContainsKey(word) ? new List<string>(index[word]) : new List<string>();

        }
        public string[] Tokenize(string text) { 
        text = text.ToLowerInvariant();
            text = Regex.Replace(text, @"[^\w\s]", "");
            return text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        }
        //testing only 
        public void PrintIndex()
        {
            foreach (var pair in index)
            {
                Console.WriteLine($"{pair.Key}: {string.Join(", ", pair.Value)}");
            }
        }
    }
}