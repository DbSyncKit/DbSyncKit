﻿namespace DbSyncKit.Core.Enum
{
    /// <summary>
    /// Represents the direction of synchronization between source and destination.
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// Indicates synchronization from source to destination only.
        /// </summary>
        SourceToDestination,

        /// <summary>
        /// Indicates synchronization from destination to source only.
        /// </summary>
        DestinationToSource,

        /// <summary>
        /// Indicates bidirectional synchronization between source and destination.
        /// </summary>
        BiDirectional
    }

}
