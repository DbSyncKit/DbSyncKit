using System.Data;
using System.Data.SqlClient;
using Sync.DB.Enum;
using Sync.DB.Interface;

namespace Sync.MSSQL
{
    public class Connection : IDatabase, IMetadata
    {
        #region Constructors

        public Connection()
        {

        }

        public Connection(string dataSource, string? initialCatalog, bool integratedSecurity, string? userID = "", string? password = "")
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

        #region SQL Querries
        private readonly string GET_TABLE_LIST = @"
            SELECT 
                name AS table_name, 
                schema_id, 
                SCHEMA_NAME(schema_id) AS schema_name, 
                type, 
                create_date, 
                modify_date, 
                max_column_id_used, 
                uses_ansi_nulls 
            FROM 
                sys.tables 
            WHERE 
                type = 'U' 
            ORDER BY 
                name;
        ";

        string GET_DATATYPE_LIST = @"
            SELECT 
                name,
                system_type_id,
                user_type_id,
                schema_id,
                principal_id,
                max_length,
                [precision],
                [scale],
                collation_name,
                is_nullable,
                is_user_defined,
                is_assembly_type,
                default_object_id,
                rule_object_id,
                is_table_type
            FROM 
                sys.types 
            ORDER BY 
                name;
        ";


        #endregion

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

        public ICollection<T> GetTables<T>()
        {
            var sqlQuerry = "";

            throw new NotImplementedException();
        }

        public ICollection<T> GetColumns<T>()
        {
            throw new NotImplementedException();
        }

        public ICollection<T> GetColumns<T>(string tableName)
        {
            throw new NotImplementedException();
        }

        public ICollection<T> GetIndex<T>()
        {
            throw new NotImplementedException();
        }

        public ICollection<T> GetPrimary<T>()
        {
            throw new NotImplementedException();
        }

        public ICollection<T> GetForeign<T>()
        {
            throw new NotImplementedException();
        }

        public ICollection<T> GetIdentity<T>()
        {
            throw new NotImplementedException();
        }

        public ICollection<T> GetUniqueConstraint<T>()
        {
            throw new NotImplementedException();
        }

        public ICollection<T> GetDefaultConstraint<T>()
        {
            throw new NotImplementedException();
        }

        public ICollection<T> GetCheckConstraint<T>()
        {
            throw new NotImplementedException();
        }

        public ICollection<T> GetUserDataType<T>()
        {
            throw new NotImplementedException();
        }

        public ICollection<T> GetUserTableType<T>()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
