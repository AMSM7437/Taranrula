using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Tarantula.Models;
using Microsoft.Data.SqlClient;
using LibDatabase;
using System.Data;

namespace Tarantula.Indexer
{
    public class TIndexer
    {
        public readonly string _connectionString;
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
                .Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);

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
                    SqlParameter[] mergeParama = new SqlParameter[]
                    {
                         new SqlParameter("@word", word.Key),
                        new SqlParameter("@docId", documentId),
                        new SqlParameter("@freq", word.Value)
                    };
                    _dBHelper.ExecuteNonQuery(
                        mergeQuery,
                        ref errMsg,
                        conn,
                        tx,
                       mergeParama
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

        private static Guid EnsureDocumentExists(
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
        // search logic goes here 
    }
}
