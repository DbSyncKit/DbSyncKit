using System.Reflection;

namespace DbSyncKit.DB.Manager
{
    /// <summary>
    /// Provides a cache manager for storing and retrieving type properties.
    /// </summary>
    public static class TypePropertyCacheManager
    {
        /// <summary>
        /// Cache to store type properties.
        /// </summary>
        private static readonly Dictionary<Type, PropertyInfo[]> TypePropertiesCache = new Dictionary<Type, PropertyInfo[]>();

        /// <summary>
        /// Gets the properties of a specified type, using a cached version if available.
        /// </summary>
        /// <param name="type">The type for which to retrieve properties.</param>
        /// <returns>An array of <see cref="PropertyInfo"/> objects representing the properties of the specified type.</returns>
        public static PropertyInfo[] GetTypeProperties(Type type)
        {
            // Check if the type's properties are already in the cache
            if (TypePropertiesCache.TryGetValue(type, out var properties))
            {
                return properties;
            }

            // Retrieve and cache the type's properties
            properties = type.GetProperties();
            TypePropertiesCache[type] = properties;

            return properties;
        }
    }
}
