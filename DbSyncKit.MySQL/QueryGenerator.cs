using System.Collections.Generic;
using System.Reflection;
using System.Text;
using DbSyncKit.DB.Helper;
using DbSyncKit.DB.Interface;
using DbSyncKit.Templates.MySql;

namespace DbSyncKit.MySQL
{
    /// <summary>
    /// Helps generate SQL queries for MySQL database operations.
    /// </summary>
    public class QueryGenerator : QueryHelper, IQueryGenerator
    {
        #region Declaration

        private QueryTemplates _template;
        private readonly string DEFAULT_SCHEMA_NAME = string.Empty;


        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="StringBuilder"/> class.
        /// </summary>
        public QueryGenerator()
        {
            _template = new QueryTemplates();
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

            bool _isMultiLine = comment.Contains(Environment.NewLine);

            return _template.COMMENT_QUERY(new
            {
                isMultiLine = _isMultiLine,
                Comment = comment
            });
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

            string tableName = GetTableName<T>();
            string schemaName = GetTableSchema<T>() ?? DEFAULT_SCHEMA_NAME;
            List<string> whereClause = GetCondition(entity, keyColumns);

            return _template.DELETE_QUERY(new
            {
                TableName = tableName,
                Where = whereClause
            });
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
            string tableName = GetTableName<T>();
            string schemaName = GetTableSchema<T>() ?? DEFAULT_SCHEMA_NAME;
            bool insertWithID = GetInsertWithID<T>();

            List<string> identityColumns = GetIdentityColumns<T>();

            Type EntityType = typeof(T);
            PropertyInfo[] properties = EntityType.GetProperties()
                .Where(prop => !excludedColumns.Contains(prop.Name) || (insertWithID && identityColumns.Contains(prop.Name)))
                .ToArray();

            List<string> columns = properties.Select(p => EscapeColumn(p.Name)).ToList();
            List<string> values = properties.Select(p => $"{EscapeValue(p.GetValue(entity))}").ToList();
            List<string> whereClause = GetCondition(entity, keyColumns);

            return _template.INSERT_QUERY(new
            {
                TableName = tableName,
                Columns = columns,
                Values = values,
                Where = whereClause
            });
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

            return _template.SELECT_QUERY(new
            {
                TableName = tableName,
                Columns = ListOfColumns,
            });

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
            string tableName = GetTableName<T>();

            List<string> setClause = editedProperties.Select(kv => $"{EscapeColumn(kv.Key)} = {EscapeValue(kv.Value)}").ToList();
            List<string> whereClause = GetCondition(DataContract, keyColumns);

            return _template.UPDATE_QUERY(new
            {
                TableName = tableName,
                Set = setClause,
                Where = whereClause
            });
        }

        /// <summary>
        /// Generates a condition for a given entity and key columns.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="entity">The entity object.</param>
        /// <param name="keyColumns">The list of key columns for the condition.</param>
        /// <returns>The generated SQL condition.</returns>
        public List<string> GetCondition<T>(T entity, List<string> keyColumns) where T : IDataContractComparer
        {
            Type entityType = typeof(T);
            PropertyInfo[] keyProperties = entityType.GetProperties().
                Where(p => keyColumns.Contains(p.Name)).ToArray();

           return keyProperties.Select(p => $"{EscapeColumn(p.Name)} = {EscapeValue(p.GetValue(entity))}").ToList();
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
