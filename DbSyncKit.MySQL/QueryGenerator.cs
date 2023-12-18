using System.Reflection;
using System.Text;

using DbSyncKit.DB.Helper;
using DbSyncKit.DB.Interface;

namespace DbSyncKit.MySQL
{
    /// <summary>
    /// Helps generate SQL queries for MySQL database operations.
    /// </summary>
    public class QueryGenerator : QueryHelper, IQueryGenerator
    {
        #region Declaration
        private StringBuilder queryBuilder;
        private readonly string DEFAULT_SCHEMA_NAME = string.Empty;

        private readonly string DELETE_QUERY = @"
DELETE FROM `@SchemaWithTableName` WHERE @Where LIMIT 1; ";

        private readonly string UPDATE_QUERY = @" 
UPDATE `@SchemaWithTableName` SET @Set WHERE @Where LIMIT 1; ";

        private readonly string INSERT_QUERY = @"
INSERT INTO `@SchemaWithTableName` (@Columns) SELECT @Values FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM `@SchemaWithTableName` WHERE @Where); ";

        private readonly string SELECT_QUERY = @"
SELECT @Columns FROM `@SchemaWithTableName`;
";

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

        /// <summary>
        /// Escapes a column name with backticks for MySQL.
        /// </summary>
        /// <param name="input">The column name to escape.</param>
        /// <returns>The escaped column name.</returns>
        public string EscapeColumn(string? input)
        {
            return $"`{input}`";
        }

        /// <summary>
        /// Escapes a value to be safely used in a SQL query.
        /// </summary>
        /// <param name="input">The value to escape.</param>
        /// <returns>The escaped value formatted for SQL.</returns>
        public object? EscapeValue(object? input)
        {
            if(input is string)
                return $"'{MySql.Data.MySqlClient.MySqlHelper.EscapeString(input as string)}'";

            return input;
        }

        /// <summary>
        /// Generates a comment for SQL queries.
        /// </summary>
        /// <param name="comment">The comment to include in the SQL query.</param>
        /// <returns>The formatted comment.</returns>
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

        /// <summary>
        /// Generates a delete query for a given entity and key columns.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="entity">The entity object.</param>
        /// <param name="keyColumns">The list of key columns for deletion.</param>
        /// <returns>The generated SQL delete query.</returns>
        public string GenerateDeleteQuery<T>(T entity, List<string> keyColumns) where T : IDataContractComparer
        {
            queryBuilder.Clear();

            string tableName = GetTableName<T>();
            string schemaName = GetTableSchema<T>() ?? DEFAULT_SCHEMA_NAME;
            string whereClause = GetCondition(entity, keyColumns);

            if(!string.IsNullOrEmpty(schemaName))
            {
                tableName = $"{EscapeColumn(schemaName)}.{EscapeColumn(tableName)}";
            }

            queryBuilder.AppendLine(DELETE_QUERY);

            ReplacePlaceholder(ref queryBuilder, "@SchemaWithTableName", tableName);
            ReplacePlaceholder(ref queryBuilder, "@Where", whereClause);

            return queryBuilder.ToString();
        }

        /// <summary>
        /// Generates an insert query for a given entity, key columns, and excluded columns.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="entity">The entity object.</param>
        /// <param name="keyColumns">The list of key columns for insertion.</param>
        /// <param name="excludedColumns">The list of columns to exclude from insertion.</param>
        /// <returns>The generated SQL insert query.</returns>
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
            string values = string.Join(", ", properties.Select(p => $"{EscapeValue(p.GetValue(entity))}"));
            string whereClause = GetCondition(entity, keyColumns);

            queryBuilder.AppendLine(INSERT_QUERY);

            if (!string.IsNullOrEmpty(schemaName))
            {
                tableName = $"{EscapeColumn(schemaName)}.{EscapeColumn(tableName)}";
            }

            ReplacePlaceholder(ref queryBuilder, "@SchemaWithTableName", tableName);
            ReplacePlaceholder(ref queryBuilder, "@Where", whereClause);
            ReplacePlaceholder(ref queryBuilder, "@Columns", columns);
            ReplacePlaceholder(ref queryBuilder, "@Values", values);

            return queryBuilder.ToString();
        }

        /// <summary>
        /// Generates a select query for a given entity and columns.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="tableName">The table name for the select query.</param>
        /// <param name="ListOfColumns">The list of columns to select.</param>
        /// <param name="schemaName">The schema name (if applicable).</param>
        /// <returns>The generated SQL select query.</returns>
        public string GenerateSelectQuery<T>(string tableName, List<string> ListOfColumns, string schemaName) where T : IDataContractComparer
        {
            if (string.IsNullOrEmpty(tableName) || ListOfColumns == null || ListOfColumns.Count == 0)
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

            queryBuilder.AppendLine(SELECT_QUERY);

            var Columns = ListOfColumns.Select(col => EscapeColumn(col));

            ReplacePlaceholder(ref queryBuilder, "@SchemaWithTableName", tableName);
            ReplacePlaceholder(ref queryBuilder, "@Columns", string.Join(", ", Columns));

            return queryBuilder.ToString();
        }

        /// <summary>
        /// Generates an update query for a given entity, key columns, excluded columns, and edited properties.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="DataContract">The entity object.</param>
        /// <param name="keyColumns">The list of key columns for updating.</param>
        /// <param name="excludedColumns">The list of columns to exclude from update.</param>
        /// <param name="editedProperties">The dictionary containing edited properties.</param>
        /// <returns>The generated SQL update query.</returns>
        public string GenerateUpdateQuery<T>(T DataContract, List<string> keyColumns, List<string> excludedColumns, Dictionary<string, object> editedProperties) where T : IDataContractComparer
        {
            queryBuilder.Clear();

            queryBuilder.AppendLine(UPDATE_QUERY);

            string tableName = GetTableName<T>();
            string schemaName = GetTableSchema<T>() ?? DEFAULT_SCHEMA_NAME;
            string setClause = string.Join(", ", editedProperties.Select(kv => $"{EscapeColumn(kv.Key)} = {EscapeValue(kv.Value)}"));
            string whereClause = GetCondition(DataContract, keyColumns);

            if (!string.IsNullOrEmpty(schemaName))
            {
                tableName = $"{EscapeColumn(schemaName)}.{EscapeColumn(tableName)}";
            }

            ReplacePlaceholder(ref queryBuilder, "@SchemaWithTableName", tableName);
            ReplacePlaceholder(ref queryBuilder, "@Where", whereClause);
            ReplacePlaceholder(ref queryBuilder, "@Set", setClause);

            return queryBuilder.ToString();
        }

        /// <summary>
        /// Generates a condition for a given entity and key columns.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="entity">The entity object.</param>
        /// <param name="keyColumns">The list of key columns for the condition.</param>
        /// <returns>The generated SQL condition.</returns>
        public string GetCondition<T>(T entity, List<string> keyColumns) where T : IDataContractComparer
        {
            Type entityType = typeof(T);
            PropertyInfo[] keyProperties = entityType.GetProperties().
                Where(p => keyColumns.Contains(p.Name)).ToArray();

            string Condition = string.Join(" AND ", keyProperties.Select(p => $"{EscapeColumn(p.Name)} = {EscapeValue(p.GetValue(entity))}"));

            return Condition;
        }

        /// <summary>
        /// Generates a batch separator for SQL queries (not used in this implementation).
        /// </summary>
        /// <returns>An empty string.</returns>

        public string GenerateBatchSeparator()
        {
            return string.Empty;
        }
    }
}
