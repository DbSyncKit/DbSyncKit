using DbSyncKit.DB.Enum;
using DbSyncKit.DB.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbSyncKit.DB.Factory
{
    /// <summary>
    /// Factory class for creating instances of <see cref="IQueryGenerator"/> based on the specified <see cref="DatabaseProvider"/>.
    /// </summary>
    public static class QueryGeneratorFactory
    {
        /// <summary>
        /// Gets an instance of <see cref="IQueryGenerator"/> based on the specified <paramref name="provider"/>.
        /// </summary>
        /// <param name="provider">The database provider for which to get the query generator.</param>
        /// <returns>An instance of <see cref="IQueryGenerator"/> for the specified database provider.</returns>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="provider"/> is not supported.</exception>
        /// <exception cref="TypeLoadException">Thrown if the type for the specified database provider is not found.</exception>
        public static IQueryGenerator GetQueryGenerator(DatabaseProvider provider)
        {
            string namespacePrefix; // Adjust this to your actual namespace
            string className = "QueryGenerator";

            switch (provider)
            {
                case DatabaseProvider.MSSQL:
                    namespacePrefix = "DbSyncKit.MSSQL";
                    break;
                case DatabaseProvider.MySQL:
                    namespacePrefix = "DbSyncKit.MySQL";
                    break;
                case DatabaseProvider.PostgreSQL:
                    namespacePrefix = "DbSyncKit.PostgreSQL";
                    break;
                // Add cases for other providers as needed
                default:
                    throw new ArgumentException("Unsupported database provider", nameof(provider));
            }

            string fullClassName = $"{namespacePrefix}.{className}, {namespacePrefix}";

            // Use reflection to create an instance of the specified class
            Type queryGeneratorType = Type.GetType(fullClassName)!;
            if (queryGeneratorType == null)
                throw new TypeLoadException($"Type {fullClassName} not found.");

            return (IQueryGenerator)Activator.CreateInstance(queryGeneratorType)!;
        }
    }
    
}
