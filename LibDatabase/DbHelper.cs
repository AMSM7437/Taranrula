using System.Data;
using Microsoft.Data.SqlClient;

namespace LibDatabase
{
    public class DBHelper
    {
        private readonly string _connectionString;
        private readonly string _masterConnection = "Server=localhost;Integrated security=True;TrustServerCertificate=True;database=master";
        public DBHelper(string connectionString)
        {
            _connectionString = connectionString;
            InitializeDatabase();
        }
        private void InitializeDatabase()
        {
            CreateDatabase();
            CreateTables();
        }
        private void CreateDatabase()
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
            int res = ExecuteNonQuery(query, _masterConnection, ref errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                Console.WriteLine("LibDatabase.DBHelper.CreateDatabase error :" + errMsg);
            }

        }
        private void CreateTables()
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
                                     Url Varchar(250) UNIQUE,
                                     Title Varchar(250),
                                     Meta Varchar(500),
                                     Text Varchar(MAX)
                                 );
                                   CREATE TABLE  tblTInvertedIndex (
                                     Word Varchar(8000),
                                     DocumentId UniqueIdentifier,
                                     Frequency INT,
                                     PRIMARY KEY (Word, DocumentId),
                                     FOREIGN KEY(DocumentId) REFERENCES tblTDocuments(Id)
                                 );
                                 CREATE INDEX idx_words ON tblTInvertedIndex(Word)
                                 END
                        END";
            int res = ExecuteNonQuery(query, _masterConnection, ref errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                Console.WriteLine("LibDatabase.DBHelper.createTables error :" + errMsg);
            }
        }

        private SqlConnection GetSqlConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public DataTable ExecuteQuery(string query, ref string errMsg, params SqlParameter[] sqlParams)
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
        public DataTable ExecuteQuery(string query, string connectionString, ref string errMsg, params SqlParameter[] sqlParams)
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
                    catch (Exception ex) { errMsg = ex.Message; }

                    return db;
                }
            }

        }

        public int ExecuteNonQuery(string query, ref string errMsg, SqlParameter[] parameters = null)
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
        public int ExecuteNonQuery(string query, ref string errMsg, SqlConnection conn, SqlTransaction transaction, SqlParameter[] parameters = null)
        {
            try
            {
                using SqlCommand cmd = new SqlCommand(query, conn, transaction);
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return -1;
            }
        }
        public int ExecuteNonQuery(string query, string connectionString, ref string errMsg, SqlParameter[] parameters = null)
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
                    catch (Exception ex)
                    {
                        errMsg = ex.Message;
                    }
                }
            }

            return result;
        }
        public bool ExecuteStoredProcedure(string procedureName, SqlParameter[] parameters = null)
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
