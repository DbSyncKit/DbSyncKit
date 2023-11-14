namespace Sync.Core.DataContract.Config
{
    /// <summary>
    /// Represents synchronization settings specific to a table.
    /// Note: Key columns and excluded columns are used for identification and exclusion respectively,
    /// but only if the specified columns are present in the table. Non-existent columns will be ignored
    /// during synchronization.
    /// </summary>
    public class TableSettings
    {
        /// <summary>
        /// Gets or sets the list of key columns used for identification in synchronization for this table.
        /// </summary>
        public List<string> KeyColumns { get; set; }

        /// <summary>
        /// Gets or sets the list of columns excluded from synchronization for this table.
        /// </summary>
        public List<string> ExcludedColumns { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether identity insert should be used during synchronization for this table.
        /// </summary>
        public bool WithIdentityInsert { get; set; }
    }
}
