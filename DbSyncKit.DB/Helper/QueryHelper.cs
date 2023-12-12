using DbSyncKit.DB.Attributes;
using DbSyncKit.DB.Interface;
using DbSyncKit.DB.Manager;
using System.Text;
using System.Text.RegularExpressions;

namespace DbSyncKit.DB.Helper
{
    /// <summary>
    /// Helper class for database queries and attribute retrieval.
    /// </summary>
    public class QueryHelper
    {
        /// <summary>
        /// Replaces a specified placeholder in a StringBuilder with the provided replacement string.
        /// </summary>
        /// <param name="stringBuilder">The StringBuilder to modify.</param>
        /// <param name="placeholder">The placeholder to replace.</param>
        /// <param name="replacement">The string to replace the placeholder.</param>
        public void ReplacePlaceholder(ref StringBuilder stringBuilder, string placeholder, string replacement)
        {
            // Use Regex to replace all occurrences of the placeholder
            string pattern = Regex.Escape(placeholder);
            stringBuilder.Replace(pattern, replacement);
        }

        /// <summary>
        /// Gets the table name of a specified type, considering the TableNameAttribute if present.
        /// </summary>
        /// <typeparam name="T">The type for which to get the table name. Must implement <see cref="IDataContractComparer"/>.</typeparam>
        /// <returns>The table name.</returns>
        public string GetTableName<T>() where T : IDataContractComparer
        {
            TableNameAttribute? tableNameAttribute = (TableNameAttribute?)Attribute.GetCustomAttribute(typeof(T), typeof(TableNameAttribute));

            if (tableNameAttribute != null)
                return tableNameAttribute.TableName;

            return typeof(T).Name;
        }

        /// <summary>
        /// Gets the table schema of a specified type, considering the TableSchemaAttribute if present.
        /// </summary>
        /// <typeparam name="T">The type for which to get the table schema. Must implement <see cref="IDataContractComparer"/>.</typeparam>
        /// <returns>The table schema name or null if not specified.</returns>
        public string? GetTableSchema<T>() where T : IDataContractComparer
        {
            TableSchemaAttribute? tableSchemaAttribute = (TableSchemaAttribute?)Attribute.GetCustomAttribute(typeof(T), typeof(TableSchemaAttribute));

            if (tableSchemaAttribute != null)
                return tableSchemaAttribute.SchemaName;

            return null;
        }

        /// <summary>
        /// Gets whether the type specifies to generate an INSERT query with ID, considering the GenerateInsertWithIDAttribute if present.
        /// </summary>
        /// <typeparam name="T">The type for which to determine the generation of INSERT query with ID. Must implement <see cref="IDataContractComparer"/>.</typeparam>
        /// <returns>True if the INSERT query should include ID, otherwise false.</returns>
        public bool GetInsertWithID<T>() where T : IDataContractComparer
        {
            GenerateInsertWithIDAttribute? tableSchemaAttribute = (GenerateInsertWithIDAttribute?)Attribute.GetCustomAttribute(typeof(T), typeof(GenerateInsertWithIDAttribute));

            if (tableSchemaAttribute != null)
                return tableSchemaAttribute.GenerateWithID;

            return false;
        }

        /// <summary>
        /// Gets whether the type specifies to include database-specific SQL statements for identity insert behavior
        /// during insert query generation, considering the GenerateInsertWithIDAttribute if present.
        /// </summary>
        /// <typeparam name="T">The type for which to determine the inclusion of identity insert statements.
        /// Must implement <see cref="IDataContractComparer"/>.</typeparam>
        /// <returns><c>true</c> if identity insert statements should be included; otherwise, <c>false</c>.</returns>
        public bool GetIncludeIdentityInsert<T>() where T : IDataContractComparer
        {
            GenerateInsertWithIDAttribute? attribute = (GenerateInsertWithIDAttribute?)Attribute.GetCustomAttribute(typeof(T), typeof(GenerateInsertWithIDAttribute));

            if (attribute != null)
                return attribute.IncludeIdentityInsert;

            return false;
        }

        /// <summary>
        /// Gets the names of properties marked as key columns for a specified type.
        /// </summary>
        /// <typeparam name="T">The type for which to get the key columns. Must implement <see cref="IDataContractComparer"/>.</typeparam>
        /// <returns>A list of key column names.</returns>
        public List<string> GetKeyColumns<T>() where T : IDataContractComparer
        {
            return TypePropertyCacheManager.GetTypeProperties(typeof(T))
                .Where(prop => Attribute.IsDefined(prop, typeof(KeyPropertyAttribute))).Select(prop => prop.Name).ToList();
        }

        /// <summary>
        /// Gets the names of properties marked as excluded properties for a specified type.
        /// </summary>
        /// <typeparam name="T">The type for which to get the excluded properties. Must implement <see cref="IDataContractComparer"/>.</typeparam>
        /// <returns>A list of excluded property names.</returns>
        public List<string> GetExcludedProperties<T>() where T : IDataContractComparer
        {
            return TypePropertyCacheManager.GetTypeProperties(typeof(T))
               .Where(prop => Attribute.IsDefined(prop, typeof(ExcludedPropertyAttribute))).Select(prop => prop.Name).ToList();
        }

        /// <summary>
        /// Gets the names of all properties for a specified type.
        /// </summary>
        /// <typeparam name="T">The type for which to get all properties. Must implement <see cref="IDataContractComparer"/>.</typeparam>
        /// <returns>A list of all property names.</returns>
        public List<string> GetAllColumns<T>() where T : IDataContractComparer
        {
            return TypePropertyCacheManager.GetTypeProperties(typeof(T))
                .Select(prop => prop.Name).ToList();
        }

    }
}
