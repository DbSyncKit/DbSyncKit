using DbSyncKit.DB.Attributes;
using DbSyncKit.DB.Enum;
using DbSyncKit.DB.Interface;
using System.Reflection;

namespace DbSyncKit.DB.Manager
{
    /// <summary>
    /// Provides a cache manager for storing and retrieving type properties.
    /// </summary>
    public static class CacheManager
    {
        #region Declerations
        /// <summary>
        /// Cache to store type properties.
        /// </summary>
        private static readonly Dictionary<Type, PropertyInfo[]> TypePropertiesCache = new Dictionary<Type, PropertyInfo[]>();

        /// <summary>
        /// Cache to store type properties in a collection, categorized by <see cref="CachePropertyType"/>.
        /// </summary>
        private static readonly Dictionary<Type, Dictionary<string,object>> TypePropertiesCacheCollection = new Dictionary<Type, Dictionary<string, object>>();

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
        /// <param name="type">The type for which to retrieve identity columns.</param>
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
                return (List<string>)identityColumns;
            }

            // If not found in the cache, retrieve and cache the identity columns
            identityColumns = CacheManager.GetTypeProperties(type)
                .Where(prop =>
                    Attribute.IsDefined(prop, typeof(KeyPropertyAttribute)) &&
                    ((KeyPropertyAttribute)Attribute.GetCustomAttribute(prop, typeof(KeyPropertyAttribute))).IsPrimaryKey
                ).Select(prop => prop.Name).ToList();


            typeCaches.Add(nameof(CachePropertyType.Identity), identityColumns);

            return (List<string>)identityColumns;
        }

        /// <summary>
        /// Gets the names of properties marked as key columns for a specified type.
        /// </summary>
        /// <param name="type">The type for which to retrieve key columns.</param>
        /// <returns>A list of key column names.</returns>
        public static List<string> GetKeyColumns(Type type)
        {
            var typeCaches = GetOrCreateDictionary(type);

            if (typeCaches.TryGetValue(nameof(CachePropertyType.Key), out var keyColumns))
            {
                return (List<string>)keyColumns;
            }

            keyColumns = GetTypeProperties(type)
                .Where(prop => 
                    Attribute.IsDefined(prop, typeof(KeyPropertyAttribute)) &&
                    ((KeyPropertyAttribute)Attribute.GetCustomAttribute(prop, typeof(KeyPropertyAttribute))).KeyProperty
                ).Select(prop => prop.Name).ToList();

            typeCaches.Add(nameof(CachePropertyType.Key), keyColumns);

            return (List<string>)keyColumns;
        }

        /// <summary>
        /// Gets the names of properties marked as excluded properties for a specified type.
        /// </summary>
        /// <param name="type">The type for which to retrieve excluded properties.</param>
        /// <returns>A list of excluded property names.</returns>
        public static List<string> GetExcludedProperties(Type type)
        {
            var typeCaches = GetOrCreateDictionary(type);

            if (typeCaches.TryGetValue(nameof(CachePropertyType.Excluded), out var excludedProperties))
            {
                return (List<string>)excludedProperties;
            }

            excludedProperties = GetTypeProperties(type)
                .Where(prop => 
                    Attribute.IsDefined(prop, typeof(ExcludedPropertyAttribute)) &&
                    ((ExcludedPropertyAttribute)Attribute.GetCustomAttribute(prop, typeof(ExcludedPropertyAttribute))).Excluded
                ).Select(prop => prop.Name).ToList();

            typeCaches.Add(nameof(CachePropertyType.Excluded), excludedProperties);

            return (List<string>)excludedProperties;
        }

        /// <summary>
        /// Gets the names of all properties for a specified type.
        /// </summary>
        /// <param name="type">The type for which to retrieve all properties.</param>
        /// <returns>A list of all property names.</returns>
        public static List<string> GetAllColumns(Type type)
        {
            var typeCaches = GetOrCreateDictionary(type);

            if (typeCaches.TryGetValue(nameof(CachePropertyType.All), out var allColumns))
            {
                return (List<string>)allColumns;
            }

            allColumns = GetTypeProperties(type).Select(prop => prop.Name).ToList();

            typeCaches.Add(nameof(CachePropertyType.All), allColumns);

            return (List<string>)allColumns;
        }

        /// <summary>
        /// Gets the name of the database table associated with the specified type.
        /// </summary>
        /// <param name="type">The type for which to retrieve the table name.</param>
        /// <returns>The name of the database table associated with the specified type.</returns>
        public static string GetTableName(Type type)
        {
            var typeCaches = GetOrCreateDictionary(type);

            if (typeCaches.TryGetValue(nameof(CachePropertyType.TableName), out var TableName))
            {
                return (string)TableName;
            }

            TableNameAttribute? tableNameAttribute = (TableNameAttribute?)Attribute.GetCustomAttribute(type, typeof(TableNameAttribute));

            if (tableNameAttribute != null)
                TableName = tableNameAttribute.TableName;
            else
                TableName = type.Name;

            typeCaches.Add(nameof(CachePropertyType.TableName), TableName);

            return (string)TableName;

        }

        /// <summary>
        /// Gets the schema of the database table associated with the specified type.
        /// </summary>
        /// <param name="type">The type for which to retrieve the table schema.</param>
        /// <returns>The schema of the database table associated with the specified type.</returns>
        public static string? GetTableSchema(Type type)
        {
            var typeCaches = GetOrCreateDictionary(type);

            if (typeCaches.TryGetValue(nameof(CachePropertyType.TableSchema), out var TableSchema))
            {
                return (string)TableSchema;
            }

            TableSchemaAttribute? tableSchemaAttribute = (TableSchemaAttribute?)Attribute.GetCustomAttribute(type, typeof(TableSchemaAttribute));

            if (tableSchemaAttribute != null)
                TableSchema = tableSchemaAttribute.SchemaName;
            else
                TableSchema = null;

            typeCaches.Add(nameof(CachePropertyType.TableSchema), TableSchema);

            return (string)TableSchema;
        }

        /// <summary>
        /// Gets a value indicating whether the INSERT query for the specified type should include the identity column.
        /// </summary>
        /// <param name="type">The type for which to determine whether to include the identity column in the INSERT query.</param>
        /// <returns><c>true</c> if the INSERT query should include the identity column; otherwise, <c>false</c>.</returns>
        public static bool GetInsertWithID(Type type)
        {
            var typeCaches = GetOrCreateDictionary(type);

            if (typeCaches.TryGetValue(nameof(CachePropertyType.GenerateWithID), out var _GenerateQueryWithID))
            {
                return (bool)_GenerateQueryWithID;
            }

            GenerateInsertWithIDAttribute? tableSchemaAttribute = (GenerateInsertWithIDAttribute?)Attribute.GetCustomAttribute(type, typeof(GenerateInsertWithIDAttribute));

            if (tableSchemaAttribute != null)
                _GenerateQueryWithID = tableSchemaAttribute.GenerateWithID;
            else
                _GenerateQueryWithID = false;

            typeCaches.Add(nameof(CachePropertyType.GenerateWithID), _GenerateQueryWithID);

            return (bool)_GenerateQueryWithID;
        }

        /// <summary>
        /// Gets a value indicating whether to include the identity insert in the INSERT query for the specified type.
        /// </summary>
        /// <param name="type">The type for which to determine whether to include the identity insert in the INSERT query.</param>
        /// <returns><c>true</c> if identity insert should be included; otherwise, <c>false</c>.</returns>
        public static bool GetIncludeIdentityInsert(Type type)
        {
            var typeCaches = GetOrCreateDictionary(type);

            if (typeCaches.TryGetValue(nameof(CachePropertyType.IncludeIdentityInsert), out var _IncludeIdentityInsertInQuery))
            {
                return (bool)_IncludeIdentityInsertInQuery;
            }

            GenerateInsertWithIDAttribute? attribute = (GenerateInsertWithIDAttribute?)Attribute.GetCustomAttribute(type, typeof(GenerateInsertWithIDAttribute));

            if (attribute != null)
                _IncludeIdentityInsertInQuery = attribute.IncludeIdentityInsert;
            else
                _IncludeIdentityInsertInQuery = false;

            typeCaches.Add(nameof(CachePropertyType.IncludeIdentityInsert), _IncludeIdentityInsertInQuery);

            return (bool)_IncludeIdentityInsertInQuery;
        }

        /// <summary>
        /// Gets the properties that are comparable (not key or excluded) for a specified type.
        /// </summary>
        /// <param name="type">The type for which to retrieve comparable properties.</param>
        /// <returns>An array of <see cref="PropertyInfo"/> objects representing comparable properties of the specified type.</returns>
        public static PropertyInfo[] GetComparableProperties(Type type)
        {
            var typeCaches = GetOrCreateDictionary(type);

            if (typeCaches.TryGetValue(nameof(CachePropertyType.ComparableProperties), out var _properties))
            {
                return (PropertyInfo[])_properties;
            }

            var keyProps = GetKeyColumns(type);
            var excludeProps = GetExcludedProperties(type);

            _properties = GetTypeProperties(type).Where(prop => !keyProps.Contains(prop.Name) && !excludeProps.Contains(prop.Name)).ToArray();

            return (PropertyInfo[])_properties;

        }

        /// <summary>
        /// Disposes of cached data associated with the specified type.
        /// </summary>
        /// <param name="type">The type for which to dispose of cached data.</param>
        public static void DisposeType(Type type)
        {
            TypePropertiesCacheCollection.Remove(type);
            TypePropertiesCache.Remove(type);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Gets or creates a dictionary for caching type-specific properties.
        /// </summary>
        /// <param name="type">The type for which to retrieve or create a dictionary.</param>
        /// <returns>A dictionary for caching type-specific properties.</returns>
        private static Dictionary<string, object> GetOrCreateDictionary(Type type)
        {
            if (!TypePropertiesCacheCollection.TryGetValue(type, out var typeCaches))
            {
                typeCaches = TypePropertiesCacheCollection[type] = new Dictionary<string, object>();
            }

            return typeCaches;
        }

        #endregion

    }
}
