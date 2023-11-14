using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sync.Core.Enum
{
    /// <summary>
    /// Represents the type of change that occurred during synchronization.
    /// </summary>
    public enum ChangeType
    {
        /// <summary>
        /// Indicates that an entity was added during synchronization.
        /// </summary>
        Added,

        /// <summary>
        /// Indicates that an entity was edited during synchronization.
        /// </summary>
        Edited,

        /// <summary>
        /// Indicates that an entity was deleted during synchronization.
        /// </summary>
        Deleted
    }

}
