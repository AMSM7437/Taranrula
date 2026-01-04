using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Tarantula.Models;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using LibDatabase;
namespace Tarantula.Indexer
{
    public class TIndexer
    {
        public readonly string _connectionString;
        private DBHelper _dBHelper;
       
        private static readonly HashSet<string> StopWords = new HashSet<string>{
               "a", "an", "and", "are", "as", "at", "be", "by", "for", "from",
               "has", "he", "in", "is", "it", "its", "of", "on", "that", "the",
               "to", "was", "were", "will", "with", "this", "but", "not", "or",
               "you", "i", "they", "we", "she", "he", "his", "her", "their" ,"nbsp"
        };
        public TIndexer()
        {
            _connectionString = "Data Source=.;Initial Catalog=TarantulaDatabase;Integrated security=True;User Id=sa;Password=sa;TrustServerCertificate=True;";
            _dBHelper = new DBHelper(_connectionString);
        }
    }
}