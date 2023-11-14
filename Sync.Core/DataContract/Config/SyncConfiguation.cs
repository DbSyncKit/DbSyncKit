namespace Sync.Core.DataContract.Config
{
    /// <summary>
    /// Represents the configuration settings for synchronization, including global settings and table-specific settings.
    /// </summary>
    public class SyncConfiguation
    {
        /// <summary>
        /// Gets or sets the global synchronization settings.
        /// </summary>
        public GlobalSettings Global { get; set; }

        /// <summary>
        /// Gets or sets the table-specific synchronization settings, organized as a dictionary where the key is the table name.
        /// </summary>
        public Dictionary<string, TableSettings> Tables { get; set; }

        /// <summary>
        /// Gets the list of excluded columns for a specific table, combining both table-specific and global exclusions.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <returns>A list of excluded column names.</returns>
        public List<string> GetExcludedList(string tableName)
        {
            TableSettings settings;
            var excluded = new List<string>();
            Tables.TryGetValue(tableName, out settings);
            {
                excluded.AddRange(settings.ExcludedColumns);
            }
            excluded.AddRange(Global.ExcludedColumns);

            return excluded;
        }

        /// <summary>
        /// Gets the list of key columns for a specific table, combining both table-specific and global key columns.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <returns>A list of key column names.</returns>
        public List<string> GetKeyColumnsList(string tableName)
        {
            TableSettings settings;
            var keyColumns = new List<string>();
            Tables.TryGetValue(tableName, out settings);
            {
                keyColumns.AddRange(settings.KeyColumns);
            }
            keyColumns.AddRange(Global.KeyColumns);

            return keyColumns;
        }
    }
}
