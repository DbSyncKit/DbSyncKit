namespace Sync.Core.DataContract
{
    /// <summary>
    /// Represents the result of a synchronization operation for a specific data type.
    /// </summary>
    /// <typeparam name="T">The type of data being synchronized.</typeparam>
    public class Result<T>
    {
        /// <summary>
        /// Gets or sets the list of entities that were added during synchronization.
        /// </summary>
        public List<T> Added { get; set; }

        /// <summary>
        /// Gets or sets the list of entities that were deleted during synchronization.
        /// </summary>
        public List<T> Deleted { get; set; }

        /// <summary>
        /// Gets or sets the list of entities that were edited during synchronization,
        /// along with a dictionary of updated properties for each edited entity.
        /// </summary>
        public List<ValueTuple<T, Dictionary<string, object>>> Edited { get; set; }

        /// <summary>
        /// Gets or sets the count of data records in the source database.
        /// </summary>
        public long SourceDataCount { get; set; }

        /// <summary>
        /// Gets or sets the count of data records in the destination database.
        /// </summary>
        public long DestinaionDataCount { get; set; }
    }
}
