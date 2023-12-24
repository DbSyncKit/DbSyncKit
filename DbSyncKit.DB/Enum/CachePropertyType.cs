using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbSyncKit.DB.Enum
{
    /// <summary>
    /// Enumerates the types of cached properties in the <see cref="CacheManager"/>.
    /// </summary>
    public enum CachePropertyType
    {
        /// <summary>
        /// Represents all properties.
        /// </summary>
        All,

        /// <summary>
        /// Represents properties marked as excluded.
        /// </summary>
        Excluded,

        /// <summary>
        /// Represents properties marked as key columns.
        /// </summary>
        Key,

        /// <summary>
        /// Represents properties marked as identity columns.
        /// </summary>
        Identity,

        /// <summary>
        /// Represents the table name associated with a type.
        /// </summary>
        TableName,

        /// <summary>
        /// Represents the table schema associated with a type.
        /// </summary>
        TableSchema,

        /// <summary>
        /// Represents whether to generate an INSERT query with an identity column.
        /// </summary>
        GenerateWithID,

        /// <summary>
        /// Represents whether to include identity insert in an INSERT query.
        /// </summary>
        IncludeIdentityInsert,

        /// <summary>
        /// Represents properties that are comparable (neither key nor excluded).
        /// </summary>
        ComparableProperties
    }

}
