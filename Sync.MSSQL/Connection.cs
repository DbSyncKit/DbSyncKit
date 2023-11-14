using Sync.DB.Enum;
using Sync.DB.Interface;
using System.Data;
using System.Data.SqlClient;

namespace Sync.MSSQL
{
    public class Connection : IDatabase
    {
        #region Constructors

        public Connection()
        {

        }

        public Connection(string dataSource, string? initialCatalog, bool integratedSecurity, string? userID, string? password)
        {
            DataSource = dataSource;
            InitialCatalog = initialCatalog;
            IntegratedSecurity = integratedSecurity;
            UserID = userID;
            Password = password;
        }

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

        public bool UseServerAddress { get; set; }
        public string? DataSource { get; set; }
        public string? Server { get; set; }
        public string? Database { get; set; }
        public string? InitialCatalog { get; set; }
        public string? UserID { get; set; }
        public string? Password { get; set; }
        public bool IntegratedSecurity { get; set; }
        public DatabaseProvider Provider => DatabaseProvider.MSSQL;

        #endregion

        #region Public Methods

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
                    catch (SqlException sqlEx)
                    {
                        throw sqlEx;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

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
        public List<T> ExecuteQuery<T>(string query, string tableName)
        {
            List<T> result = new List<T>();

            using (SqlConnection sqlConnection = new SqlConnection(this.GetConnectionString()))
            {
                try
                {
                    sqlConnection.Open();

                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection))
                    using (DataSet schema = new DataSet())
                    {
                        sqlDataAdapter.Fill(schema, tableName);

                        if (schema.Tables.Contains(tableName))
                        {
                            foreach (DataRow row in schema.Tables[tableName]!.Rows)
                            {
                                result.Add((T)Activator.CreateInstance(typeof(T), new object[] { row })!);
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

            return result;
        }

        #endregion
    }
}
