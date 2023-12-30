using DbSyncKit.DB.Interface;
using System.Reflection;

namespace DbSyncKit.Core.Comparer
{
    /// <summary>
    /// Compares instances of data contracts based on specified key properties.
    /// </summary>
    /// <typeparam name="T">Type of the data contract implementing <see cref="IDataContractComparer"/>.</typeparam>
    public class KeyEqualityComparer<T> : IEqualityComparer<T> where T : IDataContractComparer
    {
        //private readonly PropertyInfo keyProperty;

        private readonly PropertyInfo[] compariableProperties;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyEqualityComparer{T}"/> class.
        /// </summary>
        public KeyEqualityComparer(PropertyInfo[] CompariableProperties)
        {
            compariableProperties = CompariableProperties;
        }

        /// <summary>
        /// Determines whether two instances of the data contract are equal based on key properties.
        /// </summary>
        /// <param name="x">The first instance to compare.</param>
        /// <param name="y">The second instance to compare.</param>
        /// <returns><c>true</c> if the instances are equal; otherwise, <c>false</c>.</returns>
        public bool Equals(T? x, T? y)
        {
            return compariableProperties.All(prop => Equals(prop.GetValue(x), prop.GetValue(y)));
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

                foreach (var prop in compariableProperties)
                {
                    var value = prop.GetValue(obj);
                    hash = hash ^ ((value?.GetHashCode() ?? 0) + 23);
                }

                return hash;
            }
        }
    }
}
