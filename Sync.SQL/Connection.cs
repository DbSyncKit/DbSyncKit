using Sync.DB.Enum;
using Sync.DB.Interface;
using MySql.Data.MySqlClient;
using System.Data;
using Sync.Core.Utils;

namespace Sync.SQL
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

        public List<T> ExecuteQuery<T>(string query, string tableName) where T : DataContractUtility<T>
        {
            List<T> result = new List<T>();

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(GetConnectionString()))
                {
                    mySqlConnection.Open();

                    using (MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(query, mySqlConnection))
                    using (DataSet schema = new DataSet())
                    {
                        mySqlDataAdapter.Fill(schema, tableName);

                        if (schema.Tables.Contains(tableName))
                        {
                            foreach (DataRow row in schema.Tables[tableName]!.Rows)
                            {
                                result.Add((T)Activator.CreateInstance(typeof(T), new object[] { row })!);
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


    }
}
