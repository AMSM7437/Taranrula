using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Tarantula.Models;

namespace Tarantula.Indexer
{
    public class SQLiteIndexer
    {
        public readonly string connectionString;
        private static readonly HashSet<string> StopWords = new HashSet<string>
{
    "a", "an", "and", "are", "as", "at", "be", "by", "for", "from",
    "has", "he", "in", "is", "it", "its", "of", "on", "that", "the",
    "to", "was", "were", "will", "with", "this", "but", "not", "or",
    "you", "i", "they", "we", "she", "he", "his", "her", "their" ,"nbsp"

};
        public SQLiteIndexer(string databasePath)
        {
            connectionString = $"Data Source={databasePath}";
            InitializeDatabase();
        }

        private void InitializeDatabase()

        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                PRAGMA foreign_keys = ON;

                CREATE TABLE IF NOT EXISTS Documents (
                    Id TEXT PRIMARY KEY,
                    Url TEXT UNIQUE,
                    Title TEXT,
                    Meta TEXT,
                    Text TEXT
                );

                CREATE TABLE IF NOT EXISTS InvertedIndex (
                    Word TEXT,
                    DocumentId TEXT,
                    Frequency INTEGER,
                    PRIMARY KEY (Word, DocumentId),
                    FOREIGN KEY(DocumentId) REFERENCES Documents(Id)
                );

                CREATE INDEX IF NOT EXISTS idx_word ON InvertedIndex(Word);
            ";
            cmd.ExecuteNonQuery();
        }

        private string[] Tokenize(string text)
        {
            text = text.ToLowerInvariant();
            var rawTokens = Regex.Replace(text, @"[^\w\s]", "")
                                  .Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            return rawTokens.Where(token => !StopWords.Contains(token)).ToArray();
        }


        public void AddPage(PageResult page)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            var pragmaCmd = connection.CreateCommand();
            pragmaCmd.CommandText = "PRAGMA foreign_keys = ON;"; 
            pragmaCmd.Transaction = transaction;
            pragmaCmd.ExecuteNonQuery();

            var getIdCmd = connection.CreateCommand();
            getIdCmd.Transaction = transaction;
            getIdCmd.CommandText = @"SELECT Id FROM Documents WHERE Url = $url;";
            getIdCmd.Parameters.AddWithValue("$url", page.Url);
            var existingId = getIdCmd.ExecuteScalar();

            string documentId = existingId?.ToString() ?? page.Id.ToString();

            if (existingId == null)
            {
                var insertDoc = connection.CreateCommand();
                insertDoc.Transaction = transaction;
                insertDoc.CommandText = @"
            INSERT INTO Documents (Id, Url, Title, Meta, Text)
            VALUES ($id, $url, $title, $meta, $text);
        ";
                insertDoc.Parameters.AddWithValue("$id", documentId);
                insertDoc.Parameters.AddWithValue("$url", page.Url);
                insertDoc.Parameters.AddWithValue("$title", page.Title ?? "");
                insertDoc.Parameters.AddWithValue("$meta", page.Meta ?? "");
                insertDoc.Parameters.AddWithValue("$text", page.Text ?? "");
                insertDoc.ExecuteNonQuery();
            }

            var tokens = Tokenize(page.Text);
            var wordFrequencies = tokens
                .GroupBy(word => word)
                .ToDictionary(g => g.Key, g => g.Count());

            var insertWord = connection.CreateCommand();
            insertWord.Transaction = transaction;
            insertWord.CommandText = @"
        INSERT OR IGNORE INTO InvertedIndex (Word, DocumentId, Frequency)
        VALUES ($word, $docId, $freq);
    ";
            insertWord.Parameters.Add("$word", SqliteType.Text);
            insertWord.Parameters.Add("$docId", SqliteType.Text);
            insertWord.Parameters.Add("$freq", SqliteType.Integer);

            foreach (var word in wordFrequencies)
            {
                insertWord.Parameters["$word"].Value = word.Key;
                insertWord.Parameters["$docId"].Value = documentId;
                insertWord.Parameters["$freq"].Value = word.Value;
                insertWord.ExecuteNonQuery();
            }

            transaction.Commit();
        }


        public async Task<List<(string Url, string Title, string Meta, double Score)>> Search(string query)
        {
            var results = new Dictionary<string, (string Title, string Meta, double Score)>();

            string[] words = query
                .ToLowerInvariant()
                .Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(w => !StopWords.Contains(w))
                .ToArray();

            if (words.Length == 0)
                return results
       .Select(r => (r.Key, r.Value.Title, r.Value.Meta, r.Value.Score))
       .ToList();


            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var countCmd = connection.CreateCommand();
            countCmd.CommandText = "SELECT COUNT(*) FROM Documents;";
            double totalDocs = Convert.ToDouble(countCmd.ExecuteScalar());

            foreach (var word in words)
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"
            SELECT d.Url,d.Title, d.Meta,  i.Frequency,
                   (SELECT COUNT(DISTINCT DocumentId) FROM InvertedIndex WHERE Word = $word) AS DocFreq
            FROM InvertedIndex i
            JOIN Documents d ON d.Id = i.DocumentId
            WHERE i.Word = $word;
        ";
                cmd.Parameters.AddWithValue("$word", word);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string url = reader.GetString(0);
                    int tf = reader.GetInt32(3);       
                    int docFreq = reader.GetInt32(4);    


                    double idf = Math.Log((totalDocs + 1) / (1 + docFreq));
                    double tfidf = tf * idf;

                    if (results.ContainsKey(url))
                    {
                        var existing = results[url];
                        results[url] = (existing.Title, existing.Meta, existing.Score + tfidf);
                    }
                    else
                    {
                        string title = reader.GetString(1); 
                        string meta = reader.GetString(2);  
                        results[url] = (title, meta, tfidf);
                    }

                }
            }

            return results
      .OrderByDescending(r => r.Value.Score)
      .Select(r => (r.Key, r.Value.Title, r.Value.Meta, Math.Round(r.Value.Score, 4)))
      .ToList();

        }

    }
}
