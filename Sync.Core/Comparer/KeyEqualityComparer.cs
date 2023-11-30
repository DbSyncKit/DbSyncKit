using Sync.DB.Interface;

namespace Sync.Core.Comparer
{
    /// <summary>
    /// Compares instances of data contracts based on specified key properties.
    /// </summary>
    /// <typeparam name="T">Type of the data contract implementing <see cref="IDataContractComparer"/>.</typeparam>
    public class KeyEqualityComparer<T> : IEqualityComparer<T> where T : IDataContractComparer
    {
        //private readonly PropertyInfo keyProperty;

        private readonly List<string> keyColumns;
        private readonly List<string> ignoredColumns;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyEqualityComparer{T}"/> class.
        /// </summary>
        /// <param name="keyColumns">List of key property names used for comparison.</param>
        /// <param name="ignoredColumns">List of property names to be ignored during comparison.</param>
        public KeyEqualityComparer(List<string> keyColumns, List<string> ignoredColumns)
        {
            this.keyColumns = keyColumns;
            this.ignoredColumns = ignoredColumns;
        }

        /// <summary>
        /// Determines whether two instances of the data contract are equal based on key properties.
        /// </summary>
        /// <param name="x">The first instance to compare.</param>
        /// <param name="y">The second instance to compare.</param>
        /// <returns><c>true</c> if the instances are equal; otherwise, <c>false</c>.</returns>
        public bool Equals(T? x, T? y)
        {
            var properties = typeof(T).GetProperties()
                .Where(prop => keyColumns.Contains(prop.Name) && !ignoredColumns.Contains(prop.Name));

            return properties.All(prop => Equals(prop.GetValue(x), prop.GetValue(y)));
        }

        /// <summary>
        /// Returns a hash code for the specified instance of the data contract based on key properties.
        /// </summary>
        /// <param name="obj">The instance for which to get the hash code.</param>
        /// <returns>A hash code for the specified instance.</returns>
        public int GetHashCode(T obj)
        {
            unchecked
            {
                int hash = 17;
                var properties = typeof(T).GetProperties()
                    .Where(prop => keyColumns.Contains(prop.Name) && !ignoredColumns.Contains(prop.Name));

                foreach (var prop in properties)
                {
                    var value = prop.GetValue(obj);
                    hash = hash ^ ((value?.GetHashCode() ?? 0) + 23);
                }

                return hash;
            }
        }
    }
}
