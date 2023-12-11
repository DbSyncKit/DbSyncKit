namespace DbSyncKit.Core.Enum
{
    /// <summary>
    /// Represents the type of change that occurred during synchronization.
    /// </summary>
    public enum ChangeType
    {
        /// <summary>
        /// Indicates that an entity was no changes detected during synchronization.
        /// </summary>
        None,
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
        Deleted,

        /// <summary>
        /// Indicates that an entity was added and edited during synchronization.
        /// </summary>
        AddedWithEdited,

        /// <summary>
        /// Indicates that an entity was edited and deleted during synchronization.
        /// </summary>
        EditedWithDeleted,

        /// <summary>
        /// Indicates that an entity was added and deleted during synchronization.
        /// </summary>
        AddedWithDeleted,

        /// <summary>
        /// Represents all possible types of changes.
        /// </summary>
        All
    }

}
