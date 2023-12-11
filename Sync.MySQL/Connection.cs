using MySql.Data.MySqlClient;
using DbSyncKit.DB.Enum;
using DbSyncKit.DB.Interface;
using System.Data;

namespace DbSyncKit.MySQL
{
    public class Connection : IDatabase
    {
        #region Decleration

        private readonly string Server;
        private readonly int Port;
        private readonly string Database;
        private readonly string UserID;
        private readonly string Password;
        public DatabaseProvider Provider => DatabaseProvider.MySQL;


        #endregion

        #region Constructors

        public Connection(string server, int port, string database, string userID, string password)
        {
            Server = server;
            Port = port;
            Database = database;
            UserID = userID;
            Password = password;
        }

        #endregion

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
    }
}
