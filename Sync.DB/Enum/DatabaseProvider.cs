using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sync.DB.Enum
{
    /// <summary>
    /// Enumerates the supported database providers.
    /// </summary>
    public enum DatabaseProvider
    {
        /// <summary>
        /// Microsoft SQL Server.
        /// </summary>
        MSSQL,

        /// <summary>
        /// SQLite database engine.
        /// </summary>
        SQLite,

        /// <summary>
        /// MySQL database server.
        /// </summary>
        MySQL,

        /// <summary>
        /// PostgreSQL database server.
        /// </summary>
        PostgreSQL
    }

}
