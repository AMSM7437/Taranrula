using LibDatabase;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using Tarantula.Core.Classes;

namespace Tarantula.Indexer
{
    public class TIndexer
    {
        public readonly string _connectionString;
        private static readonly int _maxTokenLength = 255;
        private DBHelper _dBHelper;

        private static readonly HashSet<string> StopWords = new HashSet<string>{
            "a","an","and","are","as","at","be","by","for","from",
            "has","he","in","is","it","its","of","on","that","the",
            "to","was","were","will","with","this","but","not","or",
            "you","i","they","we","she","his","her","their","nbsp"
        };

        public TIndexer()
        {
            _connectionString =
                "Data Source=.;Initial Catalog=TarantulaDatabase;Integrated Security=True;TrustServerCertificate=True;";
            _dBHelper = new DBHelper(_connectionString);
        }

        private static string[] Tokenize(string text)
        {
            text = text.ToLowerInvariant();
            var rawTokens = Regex.Replace(text, @"[^\w\s]", "")
                .Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Where(t => t.Length <= _maxTokenLength);

            return rawTokens.Where(token => !StopWords.Contains(token)).ToArray();
        }

        public void AddPage(PageResult page)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            using SqlTransaction tx = conn.BeginTransaction();
            string errMsg = string.Empty;

            try
            {
                Guid documentId = EnsureDocumentExists(page, conn, tx);
                var tokens = Tokenize(page.Text);
                var wordFrequencies = tokens
                    .GroupBy(w => w)
                    .ToDictionary(g => g.Key, g => g.Count());
                string mergeQuery = @"
                    MERGE tblTInvertedIndex AS target
                    USING (SELECT @word AS Word, @docId AS DocumentId) AS source
                    ON target.Word = source.Word AND target.DocumentId = source.DocumentId
                    WHEN MATCHED THEN
                        UPDATE SET Frequency = @freq
                    WHEN NOT MATCHED THEN
                        INSERT (Word, DocumentId, Frequency)
                        VALUES (@word, @docId, @freq);";

                foreach (var word in wordFrequencies)
                {
                    SqlParameter[] sqlParams =
                    [
                         new("@word", word.Key),
                        new("@docId", documentId),
                        new("@freq", word.Value)
                    ];
                    _dBHelper.ExecuteNonQuery(
                        mergeQuery,
                        ref errMsg,
                        conn,
                        tx,
                        sqlParams
                    );
                }

                tx.Commit();
            }
            catch (Exception ex)
            {
                tx.Rollback();
                Console.WriteLine("AddPage failed: " + ex.Message);
            }
        }

        private static Guid EnsureDocumentExists( // modify later to use dbhelper and return single value instead of datatable in dbhelper for certain select statements
            PageResult page,
            SqlConnection connection,
            SqlTransaction transaction)
        {
            string selectQuery = @"
                SELECT Id
                FROM tblTDocuments
                WHERE Url = @url;";
            using SqlCommand selectCmd =
                new SqlCommand(selectQuery, connection, transaction);
            selectCmd.Parameters.AddWithValue("@url", page.Url);

            object? result = selectCmd.ExecuteScalar();

            if (result != null)
            {
                return (Guid)result;
            }

            Guid documentId = page.Id;

            string insertQuery = @"
                INSERT INTO tblTDocuments (Id, Url, Title, Meta, Text)
                VALUES (@id, @url, @title, @meta, @text);";

            using SqlCommand insertCmd =
                new SqlCommand(insertQuery, connection, transaction);
            insertCmd.Parameters.AddWithValue("@id", documentId);
            insertCmd.Parameters.AddWithValue("@url", page.Url);
            insertCmd.Parameters.AddWithValue("@title", page.Title ?? "");
            insertCmd.Parameters.AddWithValue("@meta", page.Meta ?? "");
            insertCmd.Parameters.AddWithValue("@text", page.Text ?? "");

            insertCmd.ExecuteNonQuery();

            return documentId;
        }
        public async Task<List<(string Url, string Title, string Meta, double Score)>> Search(string searchQuery)
        {
            string errMsg = string.Empty;
            var results = new Dictionary<string, (string Title, string Meta, double Score)>();
            string[] words = searchQuery.ToLowerInvariant().Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(w => !StopWords.Contains(w))
                .ToArray();

            if (words.Length == 0)
            {
                return results.Select(r => (r.Key, r.Value.Title, r.Value.Meta, r.Value.Score)).ToList();
            }
            string totalDocsQuery = @"SELECT COUNT(*) FROM tblTDocuments;";
            double totalDocsCount = Convert.ToDouble(_dBHelper.ExecuteScalar(totalDocsQuery, ref errMsg));
            foreach (var word in words)
            {
                string query = @" SELECT d.Url,d.Title, d.Meta,  i.Frequency,
                   (SELECT COUNT(DISTINCT DocumentId) FROM tblTInvertedIndex WHERE Word = @word) AS DocFreq
            FROM tblTInvertedIndex i
            JOIN tblTDocuments d ON d.Id = i.DocumentId
            WHERE i.Word = @word";
                SqlParameter[] searchParams = [new("@word", word)];

                DataTable res = _dBHelper.ExecuteQuery(query, ref errMsg, searchParams);
                foreach (DataRow row in res.Rows)
                {
                    string url = row["Url"].ToString()!;
                    string title = row["Title"].ToString()!;
                    string meta = row["Meta"].ToString()!;
                    int tf = Convert.ToInt32(row["Frequency"]);
                    int docFreq = Convert.ToInt32(row["DocFreq"]);
                    double idf = Math.Log((totalDocsCount + 1) / (1 + docFreq));
                    double tfidf = tf * idf;
                    if (results.ContainsKey(url))
                    {
                        var existing = results[url];
                        results[url] = (existing.Title, existing.Meta, existing.Score + tfidf);
                    }
                    else
                    {
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
