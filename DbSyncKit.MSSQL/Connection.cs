using DbSyncKit.DB.Enum;
using DbSyncKit.DB.Interface;
using System.Data;
using System.Data.SqlClient;

namespace DbSyncKit.MSSQL
{
    /// <summary>
    /// Represents a connection to a Microsoft SQL Server database.
    /// </summary>
    public class Connection : IDatabase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// </summary>
        public Connection()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class with the specified connection parameters.
        /// </summary>
        /// <param name="dataSource">The data source or IP address.</param>
        /// <param name="initialCatalog">The initial catalog or database name.</param>
        /// <param name="integratedSecurity">Indicates whether to use integrated security.</param>
        /// <param name="userID">The user ID for SQL authentication.</param>
        /// <param name="password">The password for SQL authentication.</param>
        public Connection(string dataSource, string? initialCatalog, bool integratedSecurity, string? userID = "", string? password = "")
        {
            DataSource = dataSource;
            InitialCatalog = initialCatalog;
            IntegratedSecurity = integratedSecurity;
            UserID = userID;
            Password = password;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class with the specified connection parameters.
        /// </summary>
        /// <param name="server">The server address.</param>
        /// <param name="database">The database name.</param>
        /// <param name="integratedSecurity">Indicates whether to use integrated security.</param>
        /// <param name="userID">The user ID for SQL authentication.</param>
        /// <param name="password">The password for SQL authentication.</param>
        /// <param name="useServerAddress">Indicates whether to use the server address.</param>
        public Connection(string server, string? database, bool integratedSecurity, string? userID, string? password, bool useServerAddress)
        {
            UseServerAddress = useServerAddress;
            Server = server;
            Database = database;
            IntegratedSecurity = integratedSecurity;
            UserID = userID;
            Password = password;
        }

        #endregion


        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether to use the server address.
        /// </summary>
        public bool UseServerAddress { get; set; }

        /// <summary>
        /// Gets or sets the data source or IP address.
        /// </summary>
        public string? DataSource { get; set; }

        /// <summary>
        /// Gets or sets the server address.
        /// </summary>
        public string? Server { get; set; }

        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// Gets or sets the initial catalog or database name.
        /// </summary>
        public string? InitialCatalog { get; set; }

        /// <summary>
        /// Gets or sets the user ID for SQL authentication.
        /// </summary>
        public string? UserID { get; set; }

        /// <summary>
        /// Gets or sets the password for SQL authentication.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use integrated security.
        /// </summary>
        public bool IntegratedSecurity { get; set; }

        /// <summary>
        /// Gets the database provider type, which is MSSQL for this class.
        /// </summary>
        public DatabaseProvider Provider => DatabaseProvider.MSSQL;

        #endregion

        #region Public Methods

        /// <summary>
        /// Tests the database connection.
        /// </summary>
        /// <returns>True if the connection is successful; otherwise, false.</returns>
        public bool TestConnection()
        {
            string connectionString = string.Empty;
            try
            {
                connectionString = GetConnectionString();
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    try
                    {
                        sqlConnection.Open();
                        sqlConnection.Close();
                        return true;
                    }
                    catch (SqlException)
                    {
                        throw;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the connection string based on the provided parameters.
        /// </summary>
        /// <returns>The connection string.</returns>
        public string GetConnectionString()
        {
            string connectionString = string.Empty;

            if (UseServerAddress)
            {
                if (Server == null) throw new Exception($"Server cannot be null when connecting through server address.");
                connectionString += $" Server={Server}; ";

                if (Database != null)
                    connectionString += $" Database={Database}; ";
            }
            else
            {
                if (DataSource == null) throw new Exception($"DataSource cannot be null when connecting through IP address.");
                connectionString += $" Data Source={DataSource}; ";

                if (InitialCatalog != null)
                    connectionString += $" Initial Catalog={InitialCatalog}; ";
            }

            connectionString += $" Integrated Security={IntegratedSecurity.ToString()}; ";

            if (IntegratedSecurity == false)
            {
                if (UserID == null) throw new Exception("UserID cannot be null when logging in using SQL Authentication.");
                connectionString += $" User ID={UserID}; ";

                if (Password == null) throw new Exception("Password cannot be null when logging in using SQL Authentication.");
                connectionString += $" Password={Password}; ";
            }

            return connectionString.Trim();
        }

        #endregion

        #region Static Method

        /// <summary>
        /// Executes a SQL query and returns the result as a DataSet.
        /// </summary>
        /// <param name="query">The SQL query string.</param>
        /// <param name="tableName">The name to be assigned to the result table in the DataSet.</param>
        /// <returns>A DataSet containing the result of the query.</returns>
        public DataSet ExecuteQuery(string query, string tableName)
        {
            using (SqlConnection sqlConnection = new SqlConnection(this.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();

                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection))
                    using (DataSet schema = new DataSet())
                    {
                        sqlDataAdapter.Fill(schema, tableName);

                        return schema;
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception, log it, or rethrow a more specific exception.
                    throw new Exception("Error executing query: " + ex.Message);
                }
            }
        }

        #endregion
    }
}
