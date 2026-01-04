using System.Data;
using Microsoft.Data.SqlClient;

namespace LibDatabase
{
    public class DBHelper
    {
        private readonly string _connectionString;
        private readonly string _masterConnection = "Server=localhost;Integrated security=True;User Id=sa;Password=sa;TrustServerCertificate=True;database=master";
        public DBHelper(string connectionString)
        {
            _connectionString = connectionString;
            InitializeDatabase();
        }
        private void InitializeDatabase()
        {
            createDatabase();
            createTables();
        }
        private void createDatabase()
        {
            string errMsg = string.Empty;
            string query = @"IF NOT EXISTS (
                            SELECT name
                            FROM sys.databases
                            WHERE name = N'testlast'
                        )
                        BEGIN
                            CREATE DATABASE [testlast];
                        END";
           int res = executeNonQuery(query, _masterConnection, ref errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                Console.WriteLine("LibDatabase.DBHelper.CreateDatabase error :" + errMsg);
            }
           
        }
        private void createTables()
        {
            string errMsg = string.Empty;
            string query = @"IF EXISTS (SELECT name
                            FROM sys.databases
                            WHERE name = N'testlast')
                        BEGIN
                                 USE [testlast]
                                 IF NOT EXISTS (SELECT * from sysobjects where name='tblTDocuments')
                                 BEGIN
                                 CREATE TABLE  tblTDocuments (
                                     Id UniqueIdentifier PRIMARY KEY,
                                     Url Varchar UNIQUE,
                                     Title Varchar,
                                     Meta Varchar,
                                     Text Varchar
                                 );
                                   CREATE TABLE  tblTInvertedIndex (
                                     Word Varchar,
                                     DocumentId UniqueIdentifier,
                                     Frequency INT,
                                     PRIMARY KEY (Word, DocumentId),
                                     FOREIGN KEY(DocumentId) REFERENCES tblTDocuments(Id)
                                 );
                                 CREATE INDEX idx_words ON tblTInvertedIndex(Word)
                                 END
                        END";
         int res = executeNonQuery(query, _masterConnection , ref errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                Console.WriteLine("LibDatabase.DBHelper.createTables error :" + errMsg);
            }
        }

        private SqlConnection GetSqlConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public DataTable executeQuery(string query, ref string errMsg, params SqlParameter[] sqlParams)
        {

            using (SqlConnection conn = GetSqlConnection())
            {

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    if (sqlParams != null)
                    {
                        command.Parameters.AddRange(sqlParams);

                    }
                    conn.Open();
                    DataTable db = new DataTable();

                    try
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(db);
                        }
                    }
                    catch (Exception ex) { errMsg = ex.Message; }

                    return db;
                }
            }

        }
        public DataTable executeQuery(string query, string connectionString, ref string errMsg, params SqlParameter[] sqlParams )
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    if (sqlParams != null)
                    {
                        command.Parameters.AddRange(sqlParams);

                    }
                    conn.Open();
                    DataTable db = new DataTable();

                    try
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(db);
                        }
                    }
                    catch(Exception ex) { errMsg = ex.Message; }
                   
                    return db;
                }
            }

        }

        public int executeNonQuery(string query, ref string errMsg,  SqlParameter[] parameters = null)
        {
            int result = -1;

            using (SqlConnection conn = GetSqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    conn.Open();
                    try
                    {
                        result = cmd.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        errMsg = ex.Message;
                    }
                }
            }

            return result;
        }
        public int executeNonQuery(string query,string connectionString, ref string errMsg ,  SqlParameter[] parameters = null)
        {
            int result = -1;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    conn.Open();
                    try
                    {
                        result = cmd.ExecuteNonQuery();

                    }
                    catch (Exception ex) {
                        errMsg = ex.Message;
                    }
                }
            }

            return result;
        }
        public bool executeStoredProcedure(string procedureName, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection conn = GetSqlConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(procedureName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

    }
}
