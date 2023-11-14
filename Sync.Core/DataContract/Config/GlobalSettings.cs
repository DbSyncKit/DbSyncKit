using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sync.Core.DataContract.Config
{
    /// <summary>
    /// Note: Key columns and excluded columns are used for identification and exclusion respectively,
    /// but only if the specified columns are present in a table. Non-existent columns will be ignored
    /// during synchronization.
    /// </summary>
    public class GlobalSettings
    {
        /// <summary>
        /// Gets or sets the list of key columns used for identification in synchronization across all tables.
        /// </summary>
        public List<string> KeyColumns { get; set; }

        /// <summary>
        /// Gets or sets the list of columns excluded from synchronization across all tables.
        /// </summary>
        public List<string> ExcludedColumns { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether private properties should be excluded from synchronization.
        /// Default is true.
        /// </summary>
        public bool ExcludePrivateProperties { get; set; } = true;
    }
}
