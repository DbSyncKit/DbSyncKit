using DbSyncKit.DB.Enum;
using System.Data;

namespace DbSyncKit.DB.Interface
{
    /// <summary>
    /// Defines the contract for a database connection, providing methods to retrieve the connection string
    /// and execute queries against the database for a specific data type.
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// Gets the database provider for this connection.
        /// </summary>
        DatabaseProvider Provider { get; }

        /// <summary>
        /// Retrieves the connection string for the database.
        /// </summary>
        /// <returns>The connection string.</returns>
        string GetConnectionString();

        /// <summary>
        /// Executes a query against the database
        /// </summary>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="tableName">The name of the table associated with the query.</param>
        /// <returns>A list of results of type DataSet.</returns>
        DataSet ExecuteQuery(string query, string tableName);


        /// <summary>
        /// Tests weather the connection string is valid or not
        /// </summary>
        /// <returns></returns>
        bool TestConnection();
    }
}
