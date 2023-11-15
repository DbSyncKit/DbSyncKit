using Microsoft.Data.Sqlite;
using Sync.DB.Enum;
using Sync.DB.Interface;
using Sync.DB.Utils;
using System.Data;

namespace Sync.SQLite
{
    public class Connection : IDatabase
    {
        #region Decleration

        private readonly string DataSource;
        private readonly string Version;
        public DatabaseProvider Provider => DatabaseProvider.SQLite;

        #endregion

        #region Constructor
        public Connection(string dataSource, string version)
        {
            DataSource = dataSource;
            Version = version;
        }


        #endregion

        #region Public Functions

        public DataSet ExecuteQuery(string query, string tableName)
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = query;

                        using (var reader = command.ExecuteReader())
                        {
                            DataSet dataSet = new DataSet();
                            DataTable dataTable = new DataTable(tableName);
                            dataTable.Load(reader);

                            dataSet.Tables.Add(dataTable);
                            return dataSet;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or rethrow a more specific exception.
                throw new Exception("Error executing query: " + ex.Message);
            }
        }


        public string GetConnectionString()
        {
            string connectionString = string.Empty;

            if (DataSource == null) throw new Exception($"DataSource cannot be null when connecting through IP address.");
            connectionString += $" Data Source={DataSource}; ";

            if (Version != null)
                connectionString += $" Version={Version}; ";

            return connectionString.Trim();
        }

        public bool TestConnection()
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    connection.Close();

                    return true;
                }
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or rethrow a more specific exception.
                throw new Exception("Error executing query: " + ex.Message);
            }
        }


        #endregion

    }
}
