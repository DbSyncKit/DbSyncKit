using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sync.DB.Attributes;
using Sync.DB.Helper;
using Sync.DB.Interface;

namespace Sync.MSSQL
{
    /// <summary>
    /// MSSQL Scepific Querry Generator
    /// </summary>
    public class QueryGenerator : QueryHelper,IQuerryGenerator
    {
        #region Properties

        private StringBuilder queryBuilder;

        #region Default Properties
        
        private readonly string DEFAULT_SCHEMA_NAME = "dbo";

        #endregion

        #region SQL Query Template

        private readonly string SELECT_TABLE_BASE_QUERY = "SELECT @Columns FROM @Schema.@TableName";

        private readonly string INSERT_QUERT_WITHOUT_ID = @"
        IF NOT EXISTS (SELECT 1 FROM @Schema.@TableName WHERE @Where)
        BEGIN
        INSERT INTO @Schema.@TableName (@Columns) VALUES (@Values)
        END
        ";

        private readonly string INSERT_QUERT_WITH_ID = @"
        IF NOT EXISTS (SELECT 1 FROM @Schema.@TableName WHERE @Where)
        BEGIN
            SET IDENTITY_INSERT @Schema.@TableName ON
                INSERT INTO @Schema.@TableName (@Columns) VALUES (@Values)
            SET IDENTITY_INSERT @Schema.@TableName OFF
        END
        ";

        private readonly string UPDATE_QUERY = @"
        IF EXISTS (SELECT 1 FROM @Schema.@TableName WHERE @Where)
        UPDATE @Schema.@TableName SET @Set WHERE @Where}
        ";

        private readonly string DELETE_QUERY = @"
        IF EXISTS (SELECT 1 FROM @Schema.@TableName WHERE @Where)
        DELETE FROM @Schema.@TableName WHERE @Where
        ";

        #endregion

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="StringBuilder"/> class.
        /// </summary>
        public QueryGenerator()
        {
            this.queryBuilder = new StringBuilder();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Generates a SELECT query based on the provided table name, list of columns, and schema name.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="listOfColumns">List of columns.</param>
        /// <param name="schemaName">Optional schema name, default is 'dbo'.</param>
        /// <returns>Select Query in string.</returns>
        /// <exception cref="ArgumentException">Thrown when table name or columns are null or empty.</exception>
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
            queryBuilder.Clear();

            queryBuilder.AppendLine(SELECT_TABLE_BASE_QUERY);

            var Columns = listOfColumns.Select(col => $"[{col}]");

            ReplacePlaceholder(ref queryBuilder, "@Schema", schemaName);
            ReplacePlaceholder(ref queryBuilder, "@TableName", tableName);
            ReplacePlaceholder(ref queryBuilder, "@Columns", string.Join(", ", Columns));

            //var selectQuery = string.Format(SELECT_TABLE_BASE_QUERY, Columns, schemaName, tableName);
            
            return queryBuilder.ToString();
        }

        /// <summary>
        /// Generates an UPDATE query based on the provided entity, key columns, excluded columns, and edited properties.
        /// </summary>
        /// <typeparam name="T">Type of the entity.</typeparam>
        /// <param name="DataContract">The entity data contract.</param>
        /// <param name="keyColumns">List of key columns.</param>
        /// <param name="excludedColumns">List of excluded columns.</param>
        /// <param name="editedProperties">Dictionary of edited properties.</param>
        /// <returns>Update Query in string.</returns>
        public string GenerateUpdateQuery<T>(T DataContract, List<string> keyColumns, List<string> excludedColumns, Dictionary<string, object> editedProperties)
        {
            queryBuilder.Clear();

            queryBuilder.AppendLine(UPDATE_QUERY);

            string tableName = GetTableName<T>();
            string schemaName = GetTableSchema<T>() ?? DEFAULT_SCHEMA_NAME;
            string setClause = string.Join(", ", editedProperties.Select(kv => $"{kv.Key} = '{EscapeValue(kv.Value.ToString())}'"));
            string whereClause = GetKeyCondition(DataContract, keyColumns);

            ReplacePlaceholder(ref queryBuilder, "@Schema", schemaName);
            ReplacePlaceholder(ref queryBuilder, "@TableName", tableName);
            ReplacePlaceholder(ref queryBuilder, "@Where", whereClause);
            ReplacePlaceholder(ref queryBuilder, "@Set", setClause);

            return queryBuilder.ToString();
        }

        /// <summary>
        /// Generates a DELETE query based on the provided entity and key columns.
        /// </summary>
        /// <typeparam name="T">Type of the entity.</typeparam>
        /// <param name="entity">The entity to be deleted.</param>
        /// <param name="keyColumns">List of key columns.</param>
        /// <returns>Delete Query in string.</returns>
        public string GenerateDeleteQuery<T>(T entity, List<string> keyColumns)
        {
            queryBuilder.Clear();

            string tableName = GetTableName<T>();
            string schemaName = GetTableSchema<T>() ?? DEFAULT_SCHEMA_NAME;
            string whereClause = GetKeyCondition(entity, keyColumns);

            queryBuilder.AppendLine(DELETE_QUERY);

            ReplacePlaceholder(ref queryBuilder, "@Schema", schemaName);
            ReplacePlaceholder(ref queryBuilder, "@TableName", tableName);
            ReplacePlaceholder(ref queryBuilder, "@Where", whereClause);

            return queryBuilder.ToString();
        }

        /// <summary>
        /// Generates an INSERT query based on the provided entity, key columns, and excluded columns.
        /// </summary>
        /// <typeparam name="T">Type of the entity.</typeparam>
        /// <param name="entity">The entity to be inserted.</param>
        /// <param name="keyColumns">List of key columns.</param>
        /// <param name="excludedColumns">List of excluded columns.</param>
        /// <returns>Insert Query in string.</returns>
        public string GenerateInsertQuery<T>(T entity, List<string> keyColumns, List<string> excludedColumns)
        {
            queryBuilder.Clear();

            Type EntityType = typeof(T);
            PropertyInfo[] properties = EntityType.GetProperties().Where(prop => !excludedColumns.Contains(prop.Name)).ToArray();

            string tableName = GetTableName<T>();
            string schemaName = GetTableSchema<T>() ?? DEFAULT_SCHEMA_NAME;
            bool insertWithID = GetInsertWithID<T>();
            string columns = string.Join(", ", properties.Select(p => p.Name));
            string values = string.Join(", ", properties.Select(p => $"'{EscapeValue(p.GetValue(entity)?.ToString())}'"));
            string whereClause = GetKeyCondition(entity, keyColumns);

            if (insertWithID)
                queryBuilder.AppendLine(INSERT_QUERT_WITH_ID);
            else
                queryBuilder.AppendLine(INSERT_QUERT_WITHOUT_ID);

            ReplacePlaceholder(ref queryBuilder, "@Schema", schemaName);
            ReplacePlaceholder(ref queryBuilder, "@TableName", tableName);
            ReplacePlaceholder(ref queryBuilder, "@Where", whereClause);
            ReplacePlaceholder(ref queryBuilder, "@Columns", columns);
            ReplacePlaceholder(ref queryBuilder, "@Values", values);

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
            string keyCondition = string.Join(" AND ", keyProperties.Select(p => $"{p.Name} = '{EscapeValue(p.GetValue(entity)?.ToString())}'"));

            return keyCondition;
        }

        private string? EscapeValue(string? input)
        {
            return input?.Replace("'", "''");
        }


        #endregion
    }
}
