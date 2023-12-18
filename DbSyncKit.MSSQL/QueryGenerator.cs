using DbSyncKit.DB.Helper;
using DbSyncKit.DB.Interface;
using System.Reflection;
using System.Text;

namespace DbSyncKit.MSSQL
{
    /// <summary>
    /// MSSQL Scepific Querry Generator
    /// </summary>
    public class QueryGenerator : QueryHelper, IQueryGenerator
    {
        #region Properties

        private StringBuilder queryBuilder;

        #region Default Properties

        private readonly string DEFAULT_SCHEMA_NAME = "dbo";

        #endregion

        #region SQL Query Template

        private readonly string SELECT_TABLE_BASE_QUERY = " SELECT @Columns FROM @Schema.@TableName ";

        private readonly string INSERT_QUERY_WITHOUT_ID = @" 
IF NOT EXISTS (SELECT 1 FROM @Schema.@TableName WHERE @Where)
BEGIN
INSERT INTO @Schema.@TableName (@Columns) VALUES (@Values)
END ";

        private readonly string INSERT_QUERY_WITH_ID = @" 
IF NOT EXISTS (SELECT 1 FROM @Schema.@TableName WHERE @Where)
BEGIN
    SET IDENTITY_INSERT @Schema.@TableName ON
        INSERT INTO @Schema.@TableName (@Columns) VALUES (@Values)
    SET IDENTITY_INSERT @Schema.@TableName OFF
END ";
        private readonly string INSERT_QUERY_WITHOUT_IDENTITY_INSERT_WITH_ID = @" 
IF NOT EXISTS (SELECT 1 FROM @Schema.@TableName WHERE @Where)
BEGIN
    INSERT INTO @Schema.@TableName (@Columns) VALUES (@Values)
END ";

        private readonly string UPDATE_QUERY = @" 
IF EXISTS (SELECT 1 FROM @Schema.@TableName WHERE @Where)
UPDATE @Schema.@TableName SET @Set WHERE @Where ";

        private readonly string DELETE_QUERY = @" 
IF EXISTS (SELECT 1 FROM @Schema.@TableName WHERE @Where)
DELETE FROM @Schema.@TableName WHERE @Where ";

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
        public string GenerateSelectQuery<T>(string tableName, List<string> listOfColumns, string schemaName) where T : IDataContractComparer
        {
            if (string.IsNullOrEmpty(tableName) || listOfColumns == null || listOfColumns.Count == 0)
            {
                throw new ArgumentException("Table name and columns cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(tableName))
            {
                tableName = GetTableName<T>();
            }

            if (string.IsNullOrEmpty(schemaName))
            {
                schemaName = GetTableSchema<T>() ?? DEFAULT_SCHEMA_NAME;
            }
            queryBuilder.Clear();

            queryBuilder.AppendLine(SELECT_TABLE_BASE_QUERY);

            var Columns = listOfColumns.Select(col => EscapeColumn(col));

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
        public string GenerateUpdateQuery<T>(T DataContract, List<string> keyColumns, List<string> excludedColumns, Dictionary<string, object> editedProperties) where T : IDataContractComparer
        {
            queryBuilder.Clear();

            queryBuilder.AppendLine(UPDATE_QUERY);

            string tableName = GetTableName<T>();
            string schemaName = GetTableSchema<T>() ?? DEFAULT_SCHEMA_NAME;
            string setClause = string.Join(", ", editedProperties.Select(kv => $"{EscapeColumn(kv.Key)} = '{EscapeValue(kv.Value)}'"));
            string whereClause = GetCondition(DataContract, keyColumns);

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
        public string GenerateDeleteQuery<T>(T entity, List<string> keyColumns) where T : IDataContractComparer
        {
            queryBuilder.Clear();

            string tableName = GetTableName<T>();
            string schemaName = GetTableSchema<T>() ?? DEFAULT_SCHEMA_NAME;
            string whereClause = GetCondition(entity, keyColumns);

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
        public string GenerateInsertQuery<T>(T entity, List<string> keyColumns, List<string> excludedColumns) where T : IDataContractComparer
        {
            queryBuilder.Clear();

            Type EntityType = typeof(T);
            PropertyInfo[] properties = EntityType.GetProperties().Where(prop => !excludedColumns.Contains(prop.Name)).ToArray();

            string tableName = GetTableName<T>();
            string schemaName = GetTableSchema<T>() ?? DEFAULT_SCHEMA_NAME;
            bool insertWithID = GetInsertWithID<T>();
            bool identityInsert = GetIncludeIdentityInsert<T>();
            string columns = string.Join(", ", properties.Select(p => EscapeColumn(p.Name)));
            string values = string.Join(", ", properties.Select(p => $"'{EscapeValue(p.GetValue(entity))}'"));
            string whereClause = GetCondition(entity, keyColumns);

            if (insertWithID && identityInsert)
                queryBuilder.AppendLine(INSERT_QUERY_WITH_ID);
            else if(identityInsert && !identityInsert)
                queryBuilder.AppendLine(INSERT_QUERY_WITHOUT_IDENTITY_INSERT_WITH_ID);
            else
                queryBuilder.AppendLine(INSERT_QUERY_WITHOUT_ID);

            ReplacePlaceholder(ref queryBuilder, "@Schema", schemaName);
            ReplacePlaceholder(ref queryBuilder, "@TableName", tableName);
            ReplacePlaceholder(ref queryBuilder, "@Where", whereClause);
            ReplacePlaceholder(ref queryBuilder, "@Columns", columns);
            ReplacePlaceholder(ref queryBuilder, "@Values", values);

            return queryBuilder.ToString();
        }

        /// <summary>
        /// Generates a comment string in either single-line or multi-line format.
        /// </summary>
        /// <param name="comment">The content of the comment. If the comment contains line breaks, it will be treated as a multi-line comment; otherwise, it will be treated as a single-line comment.</param>
        /// <returns>The generated comment string.</returns>

        public string GenerateComment(string comment)
        {
            if (string.IsNullOrWhiteSpace(comment))
                return string.Empty;

            bool isMultiLine = comment.Contains(Environment.NewLine);

            if (!isMultiLine)
                return "-- " + comment;


            StringBuilder CommentBuilder = new StringBuilder();
            CommentBuilder.AppendLine("/*");
            CommentBuilder.AppendLine(comment);
            CommentBuilder.AppendLine("*/");

            return CommentBuilder.ToString();
        }

        #endregion

        #region Helper Methods
        /// <summary>
        /// Generates a SQL WHERE clause based on the specified entity and key columns.
        /// </summary>
        /// <typeparam name="T">The type of the entity that implements IDataContractComparer.</typeparam>
        /// <param name="entity">The entity for which the condition is generated.</param>
        /// <param name="keyColumns">The list of key columns used to create the condition.</param>
        /// <returns>A string representing the SQL WHERE clause based on the key columns of the entity.</returns>
        public string GetCondition<T>(T entity, List<string> keyColumns) where T : IDataContractComparer
        {
            Type entityType = typeof(T);
            PropertyInfo[] keyProperties = entityType.GetProperties().
                Where(p => keyColumns.Contains(p.Name)).ToArray();

            string Condition = string.Join(" AND ", keyProperties.Select(p => $"{EscapeColumn(p.Name)} = '{EscapeValue(p.GetValue(entity))}'"));

            return Condition;
        }

        /// <summary>
        /// Escapes special characters in the input string to make it SQL-safe.
        /// </summary>
        /// <param name="input">The input object or string to be escaped.</param>
        /// <returns>The escaped object or string.</returns>
        public object? EscapeValue(object? input)
        {
            if (input != null && input is string && (input as string)!.Contains("'"))
                return (input as string)!.Replace("'", "''");

            return input;
        }

        /// <summary>
        /// Escapes the input column name to be used safely in SQL queries.
        /// </summary>
        /// <param name="input">The input column name to be escaped.</param>
        /// <returns>The escaped column name enclosed in square brackets.</returns>
        public string EscapeColumn(string? input)
        {
            return $"[{input}]";
        }

        /// <summary>
        /// Generates a SQL batch separator ('GO' statement in MSSQL) used to execute batches of SQL statements in MSSQL environments.
        /// </summary>
        /// <returns>A string representing the generated batch separator ('GO' statement).</returns>
        public string GenerateBatchSeparator()
        {
            return " GO ";
        }

        #endregion
    }
}
