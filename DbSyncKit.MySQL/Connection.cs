using MySql.Data.MySqlClient;
using DbSyncKit.DB.Enum;
using DbSyncKit.DB.Interface;
using System.Data;

namespace DbSyncKit.MySQL
{
    /// <summary>
    /// Represents a MySQL database connection implementing the IDatabase interface for general database operations.
    /// </summary>
    public class Connection : IDatabase
    {
        #region Decleration

        private readonly string Server;
        private readonly int Port;
        private readonly string Database;
        private readonly string UserID;
        private readonly string Password;

        /// <summary>
        /// Gets the database provider type, which is MySql for this class.
        /// </summary>
        public DatabaseProvider Provider => DatabaseProvider.MySQL;


        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the Connection class with MySQL server details.
        /// </summary>
        /// <param name="server">The MySQL server name or IP address.</param>
        /// <param name="port">The port number for the MySQL server.</param>
        /// <param name="database">The name of the MySQL database.</param>
        /// <param name="userID">The user ID to authenticate with the MySQL server.</param>
        /// <param name="password">The password to authenticate with the MySQL server.</param>
        public Connection(string server, int port, string database, string userID, string password)
        {
            Server = server;
            Port = port;
            Database = database;
            UserID = userID;
            Password = password;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the connection string for the MySQL database using provided server details.
        /// </summary>
        /// <returns>A string representing the MySQL connection string.</returns>
        public string GetConnectionString()
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
            {
                Server = Server,
                Port = (uint)Port,
                Database = Database,
                UserID = UserID,
                Password = Password,
                ConnectionTimeout = 30 // Set your desired connection timeout
            };

            return builder.ConnectionString;
        }

        /// <summary>
        /// Executes a query against the MySQL database and returns the results as a DataSet.
        /// </summary>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="tableName">The name to assign to the resulting table within the DataSet.</param>
        /// <returns>A DataSet containing the results of the query.</returns>
        public DataSet ExecuteQuery(string query, string tableName)
        {
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(GetConnectionString()))
                {
                    mySqlConnection.Open();

                    using (MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(query, mySqlConnection))
                    using (DataSet dataset = new DataSet())
                    {
                        mySqlDataAdapter.Fill(dataset, tableName);

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
        /// Tests the connection to the MySQL database.
        /// </summary>
        /// <returns>True if the connection is successful; otherwise, false.</returns>
        public bool TestConnection()
        {
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(GetConnectionString()))
                {
                    mySqlConnection.Open();
                    mySqlConnection.Close();

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
