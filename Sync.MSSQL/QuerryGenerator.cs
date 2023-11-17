using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Sync.DB.Interface;

namespace Sync.MSSQL
{
    /// <summary>
    /// MSSQL Scepific Querry Generator
    /// </summary>
    public class QuerryGenerator : IQuerryGenerator
    {
        #region Properties
        private readonly string SELECT_TABLE_BASE_QUERY = "SELECT {0} FROM {1}.{2}";
        #endregion

        #region Public Methods

        /// <summary>
        /// This will return Select query for you from the available data.
        /// </summary>
        /// <param name="tableName">Name of the table</param>
        /// <param name="listOfColumns">List of Columns</param>
        /// <param name="schemaName">Optional Schema name like dbo</param>
        /// <returns>Select Query in string</returns>
        /// <exception cref="ArgumentException"></exception>
        public string GenerateSelectQuery(string tableName, List<string> listOfColumns, string schemaName)
        {
            if (string.IsNullOrEmpty(tableName) || listOfColumns == null || listOfColumns.Count == 0)
            {
                throw new ArgumentException("Table name and columns cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(schemaName))
            {
                schemaName = "dbo";
            }

            var selectQuery = string.Format(SELECT_TABLE_BASE_QUERY, listOfColumns.Select(col => $"[{col}]"), schemaName, tableName);
            
            return selectQuery;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="DataContract"></param>
        /// <param name="keyColumns"></param>
        /// <param name="excludedColumns"></param>
        /// <param name="editedProperties"></param>
        /// <returns></returns>

        public string GenerateUpdateQuery<T>(T DataContract, List<string> keyColumns, List<string> excludedColumns, Dictionary<string, object> editedProperties)
        {
            var queryBuilder = new StringBuilder();

            Type entityType = typeof(T);
            PropertyInfo[] properties = entityType.GetProperties().Where(prop => !excludedColumns.Contains(prop.Name)).ToArray();

            string tableName = entityType.Name;
            string setClause = string.Join(", ", editedProperties.Select(kv => $"{kv.Key} = '{kv.Value.ToString()?.Replace("'", "''")}'"));
            string whereClause = GetKeyCondition(DataContract, keyColumns);

            queryBuilder.AppendLine($"IF EXISTS (SELECT 1 FROM {tableName} WHERE {whereClause})");
            queryBuilder.AppendLine($"UPDATE {tableName} SET {setClause} WHERE {whereClause}");

            return queryBuilder.ToString();
        }

        #endregion

        #region Private Methods

        private string GetKeyCondition<T>(T entity, List<string> keyColumns)
        {
            Type entityType = typeof(T);
            PropertyInfo[] properties = entityType.GetProperties();

            var keyproperty = properties.FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(KeyAttribute)))!;

            if (keyproperty != null)
                keyColumns.Add(keyproperty.Name);

            var keyProperties = properties.Where(p => keyColumns.Contains(p.Name));
            string keyCondition = string.Join(" AND ", keyProperties.Select(p => $"{p.Name} = '{p.GetValue(entity)?.ToString()?.Replace("'", "''")}'"));

            return keyCondition;
        }

        #endregion
    }
}
