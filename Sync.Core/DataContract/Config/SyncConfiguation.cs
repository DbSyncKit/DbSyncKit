using Sync.DB.Interface;
using Sync.DB.Utils;

namespace Sync.Core.DataContract.Config
{
    /// <summary>
    /// Represents the configuration settings for synchronization, including global settings and table-specific settings.
    /// </summary>
    public static class SyncConfiguation
    {
        /// <summary>
        /// Gets or sets the global synchronization settings.
        /// </summary>
        public static GlobalSettings Global { get; set; }

        /// <summary>
        /// Gets or sets the table-specific synchronization settings, organized as a dictionary where the key is the table name.
        /// </summary>
        public static Dictionary<string, TableSettings> Tables { get; set; }

        /// <summary>
        /// Gets the all datacontracts that are inheriting DataContractUtility at runtime
        /// </summary>
        public static Dictionary<string, string> DataContractList { get; set; } = FindDataContractClasses();

        /// <summary>
        /// Finds all classes that inherit from DataContractUtility&lt;&gt; across all loaded assemblies.
        /// </summary>
        /// <returns>
        /// A dictionary where the keys are class names and the values are their respective namespaces.
        /// </returns>
        public static Dictionary<string, string> FindDataContractClasses()
        {
            var dataContractClasses = new Dictionary<string, string>();

            // Get all loaded assemblies
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Iterate through each assembly
            foreach (var assembly in assemblies)
            {
                // Get all types in the assembly
                var types = assembly.GetTypes();

                // Filter types that inherit from DataContractUtility<T>
                var dataContractTypes = types
                .Where(type =>
                    type.IsClass &&
                    !type.IsAbstract &&
                    type.BaseType != null &&
                    (type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(DataContractUtility<>) ||
                    type.GetInterfaces().Any(i => i == typeof(IDataContractComparer)))
                );

                // Add the found types to the dictionary
                foreach (var dataContractType in dataContractTypes)
                {
                    dataContractClasses[dataContractType.Name] = dataContractType.Namespace;
                }
            }

            return dataContractClasses;
        }

        /// <summary>
        /// Gets the list of excluded columns for a specific table, combining both table-specific and global exclusions.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <returns>A list of excluded column names.</returns>
        public static List<string> GetExcludedList(string tableName)
        {
            TableSettings settings;
            var excluded = new List<string>();
            Tables.TryGetValue(tableName, out settings);
            {
                excluded.AddRange(settings.ExcludedColumns);
            }
            excluded.AddRange(Global.ExcludedColumns);

            return excluded;
        }

        /// <summary>
        /// Gets the list of key columns for a specific table, combining both table-specific and global key columns.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <returns>A list of key column names.</returns>
        public static List<string> GetKeyColumnsList(string tableName)
        {
            TableSettings settings;
            var keyColumns = new List<string>();
            Tables.TryGetValue(tableName, out settings);
            {
                keyColumns.AddRange(settings.KeyColumns);
            }
            keyColumns.AddRange(Global.KeyColumns);

            return keyColumns;
        }
    }
}
