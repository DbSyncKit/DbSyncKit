using Sync.DB.Enum;
using Sync.DB.Interface;
using Microsoft.Data.Sqlite;
using System.Data;
using Sync.Core.Utils;

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

        public List<T> ExecuteQuery<T>(string query, string tableName) where T : DataContractUtility<T>
        {
            List<T> result = new List<T>();

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
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);

                            if (dataTable.Rows.Count > 0)
                            {
                                foreach (DataRow row in dataTable.Rows)
                                {
                                    result.Add((T)Activator.CreateInstance(typeof(T), new object[] { row })!);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or rethrow a more specific exception.
                throw new Exception("Error executing query: " + ex.Message);
            }

            return result;
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


        #endregion

    }
}
