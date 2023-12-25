using DbSyncKit.DB.Enum;
using DbSyncKit.DB.Interface;
using Npgsql;
using System.Data;

namespace DbSyncKit.PostgreSQL
{
    public class Connection : IDatabase
    {
        #region Declaration

        private readonly string Host;
        private readonly int Port;
        private readonly string Database;
        private readonly string UserID;
        private readonly string Password;

        /// <summary>
        /// Gets the database provider type, which is PostgreSQL for this class.
        /// </summary>
        public DatabaseProvider Provider => DatabaseProvider.PostgreSQL;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Connection class with PostgreSQL server details.
        /// </summary>
        /// <param name="host">The PostgreSQL server name or IP address.</param>
        /// <param name="port">The port number for the PostgreSQL server.</param>
        /// <param name="database">The name of the PostgreSQL database.</param>
        /// <param name="userID">The user ID to authenticate with the PostgreSQL server.</param>
        /// <param name="password">The password to authenticate with the PostgreSQL server.</param>
        public Connection(string host, int port, string database, string userID, string password)
        {
            Host = host;
            Port = port;
            Database = database;
            UserID = userID;
            Password = password;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the connection string for the PostgreSQL database using provided server details.
        /// </summary>
        /// <returns>A string representing the PostgreSQL connection string.</returns>
        public string GetConnectionString()
        {
            NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder
            {
                Host = Host,
                Port = Port,
                Database = Database,
                Username = UserID,
                Password = Password,
                Timeout = 30 // Set your desired connection timeout
            };

            return builder.ConnectionString;
        }

        /// <summary>
        /// Executes a query against the PostgreSQL database and returns the results as a DataSet.
        /// </summary>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="tableName">The name to assign to the resulting table within the DataSet.</param>
        /// <returns>A DataSet containing the results of the query.</returns>
        public DataSet ExecuteQuery(string query, string tableName)
        {
            try
            {
                using (NpgsqlConnection npgSqlConnection = new NpgsqlConnection(GetConnectionString()))
                {
                    npgSqlConnection.Open();

                    using (NpgsqlDataAdapter npgSqlDataAdapter = new NpgsqlDataAdapter(query, npgSqlConnection))
                    using (DataSet dataset = new DataSet())
                    {
                        npgSqlDataAdapter.Fill(dataset, tableName);
                        return dataset;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or rethrow a more specific exception.
                throw new Exception("Error executing query: " + ex.Message);
            }
        }

        /// <summary>
        /// Tests the connection to the PostgreSQL database.
        /// </summary>
        /// <returns>True if the connection is successful; otherwise, false.</returns>
        public bool TestConnection()
        {
            try
            {
                using (NpgsqlConnection npgSqlConnection = new NpgsqlConnection(GetConnectionString()))
                {
                    npgSqlConnection.Open();
                    npgSqlConnection.Close();

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
