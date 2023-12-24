using DbSyncKit.DB.Attributes;
using DbSyncKit.DB.Enum;
using DbSyncKit.DB.Interface;
using System.Reflection;

namespace DbSyncKit.DB.Manager
{
    /// <summary>
    /// Provides a cache manager for storing and retrieving type properties.
    /// </summary>
    public static class TypePropertyCacheManager
    {
        #region Declerations
        /// <summary>
        /// Cache to store type properties.
        /// </summary>
        private static readonly Dictionary<Type, PropertyInfo[]> TypePropertiesCache = new Dictionary<Type, PropertyInfo[]>();

        /// <summary>
        /// Cache to store type properties in list.
        /// </summary>
        private static readonly Dictionary<Type, Dictionary<string,List<string>>> TypePropertiesCacheCollection = new Dictionary<Type, Dictionary<string, List<string>>>();

        #endregion

        #region Cache Methods

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

        /// <summary>
        /// Retrieves a list of identity columns for a specified data contract type.
        /// </summary>
        /// <returns>A list containing the names of identity columns for the specified data contract type.</returns>
        /// <remarks>
        /// This method uses reflection to analyze the properties of the specified type and retrieves properties marked with a [Key] attribute, indicating identity columns.
        /// </remarks>
        public static List<string> GetIdentityColumns(Type type)
        {
            var typeCaches = GetOrCreateDictionary(type);
            // Try to retrieve the identity columns from the cache
            if (typeCaches.TryGetValue(nameof(CachePropertyType.Identity), out var identityColumns))
            {
                return identityColumns;
            }

            // If not found in the cache, retrieve and cache the identity columns
            identityColumns = TypePropertyCacheManager.GetTypeProperties(type)
                .Where(prop =>
                    Attribute.IsDefined(prop, typeof(KeyPropertyAttribute)) &&
                    ((KeyPropertyAttribute)Attribute.GetCustomAttribute(prop, typeof(KeyPropertyAttribute))).IsPrimaryKey
                ).Select(prop => prop.Name).ToList();


            typeCaches.Add(nameof(CachePropertyType.Identity), identityColumns);

            return identityColumns;
        }

        /// <summary>
        /// Gets the names of properties marked as key columns for a specified type.
        /// </summary>
        /// <returns>A list of key column names.</returns>
        public static List<string> GetKeyColumns(Type type)
        {
            var typeCaches = GetOrCreateDictionary(type);

            if (typeCaches.TryGetValue(nameof(CachePropertyType.Key), out var keyColumns))
            {
                return keyColumns;
            }

            keyColumns = GetTypeProperties(type)
                .Where(prop => 
                    Attribute.IsDefined(prop, typeof(KeyPropertyAttribute)) &&
                    ((KeyPropertyAttribute)Attribute.GetCustomAttribute(prop, typeof(KeyPropertyAttribute))).KeyProperty
                ).Select(prop => prop.Name).ToList();

            typeCaches.Add(nameof(CachePropertyType.Key), keyColumns);

            return keyColumns;
        }

        /// <summary>
        /// Gets the names of properties marked as excluded properties for a specified type.
        /// </summary>
        /// <returns>A list of excluded property names.</returns>
        public static List<string> GetExcludedProperties(Type type)
        {
            var typeCaches = GetOrCreateDictionary(type);

            if (typeCaches.TryGetValue(nameof(CachePropertyType.Excluded), out var excludedProperties))
            {
                return excludedProperties;
            }

            excludedProperties = GetTypeProperties(type)
                .Where(prop => 
                    Attribute.IsDefined(prop, typeof(ExcludedPropertyAttribute)) &&
                    ((ExcludedPropertyAttribute)Attribute.GetCustomAttribute(prop, typeof(ExcludedPropertyAttribute))).Excluded
                ).Select(prop => prop.Name).ToList();

            typeCaches.Add(nameof(CachePropertyType.Excluded), excludedProperties);

            return excludedProperties;
        }

        /// <summary>
        /// Gets the names of all properties for a specified type.
        /// </summary>
        /// <returns>A list of all property names.</returns>
        public static List<string> GetAllColumns(Type type)
        {
            var typeCaches = GetOrCreateDictionary(type);

            if (typeCaches.TryGetValue(nameof(CachePropertyType.All), out var allColumns))
            {
                return allColumns;
            }

            allColumns = GetTypeProperties(type).Select(prop => prop.Name).ToList();

            typeCaches.Add(nameof(CachePropertyType.All), allColumns);

            return allColumns;
        }
        #endregion

        #region Private Methods
        private static Dictionary<string, List<string>> GetOrCreateDictionary(Type type)
        {
            if (!TypePropertiesCacheCollection.TryGetValue(type, out var typeCaches))
            {
                typeCaches = TypePropertiesCacheCollection[type] = new Dictionary<string, List<string>>();
            }

            return typeCaches;
        }

        #endregion

    }
}
