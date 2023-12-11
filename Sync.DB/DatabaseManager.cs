using DbSyncKit.DB.Interface;
using System.Data;


namespace DbSyncKit.DB
{
    /// <summary>
    /// Manages database operations for a specific database provider implementing the IDatabase interface.
    /// </summary>
    /// <typeparam name="T">Type of the database provider implementing IDatabase.</typeparam>
    public class DatabaseManager<T> : IDisposable where T : IDatabase
    {
        private readonly T _databaseProvider;

        /// <summary>
        /// Initializes a new instance of the DatabaseManager class.
        /// </summary>
        /// <param name="databaseProvider">The instance of the database provider.</param>
        public DatabaseManager(T databaseProvider)
        {
            _databaseProvider = databaseProvider ?? throw new ArgumentNullException(nameof(databaseProvider));
        }

        /// <summary>
        /// Executes a query against the database and returns a list of results for a specific data type.
        /// </summary>
        /// <typeparam name="TItem">The type of data to retrieve, constrained to be an instance of DataContractUtility.</typeparam>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="tableName">The name of the table associated with the query.</param>
        /// <returns>A list of results of type <typeparamref name="TItem"/>.</returns>
        public List<TItem> ExecuteQuery<TItem>(string query, string tableName) where TItem : IDataContractComparer
        {
            List<TItem> result = new List<TItem>();

            var dataset = _databaseProvider.ExecuteQuery(query, tableName);

            if (dataset.Tables.Contains(tableName))
            {
                foreach (DataRow row in dataset.Tables[tableName]!.Rows)
                {
                    result.Add((TItem)Activator.CreateInstance(typeof(TItem), new object[] { row })!);
                }
            }

            return result;
        }

        /// <summary>
        /// Disposes of the DatabaseManager.
        /// </summary>
        public void Dispose()
        {
            // No additional cleanup required
        }
    }
}
