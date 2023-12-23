using DbSyncKit.DB.Helper;
using DbSyncKit.DB.Interface;
using DbSyncKit.Templates.MSSQL;
using DotLiquid;
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
        private QueryTemplates _template;

        #region Default Properties

        private readonly string DEFAULT_SCHEMA_NAME = "dbo";

        #endregion

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

            return _template.SELECT_QUERY.Render(Hash.FromAnonymousObject(new
            {
                TableName = tableName,
                Schema = schemaName,
                Columns = listOfColumns
            }));
            
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
            string tableName = GetTableName<T>();
            string schemaName = GetTableSchema<T>() ?? DEFAULT_SCHEMA_NAME;
            List<string> SetClause = editedProperties.Select(kv => $"{EscapeColumn(kv.Key)} = '{EscapeValue(kv.Value)}'").ToList();
            List<string> condition = GetCondition(DataContract, keyColumns);

            return _template.UPDATE_QUERY.Render(Hash.FromAnonymousObject(new
            {
                TableName = tableName,
                Schema = schemaName,
                Set = SetClause,
                Where = condition
            }));
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
            string tableName = GetTableName<T>();
            string schemaName = GetTableSchema<T>() ?? DEFAULT_SCHEMA_NAME;
            List<string> condition = GetCondition(entity, keyColumns);

            return _template.DELETE_QUERY.Render(Hash.FromAnonymousObject(new
            {
                TableName = tableName,
                Schema = schemaName,
                Where = condition
            }));
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
            string tableName = GetTableName<T>();
            string schemaName = GetTableSchema<T>() ?? DEFAULT_SCHEMA_NAME;
            bool insertWithID = GetInsertWithID<T>();
            bool identityInsert = GetIncludeIdentityInsert<T>();
            List<string> condition = GetCondition(entity, keyColumns);
            List<string> identityColumns = GetIdentityColumns<T>();

            Type EntityType = typeof(T);
            PropertyInfo[] properties = EntityType.GetProperties().Where(prop => !excludedColumns.Contains(prop.Name) || (insertWithID && identityColumns.Contains(prop.Name))).ToArray();

            List<string> columns = properties.Select(p => EscapeColumn(p.Name)).ToList();
            List<string> values = properties.Select(p => $"'{EscapeValue(p.GetValue(entity))}'").ToList();

            return _template.INSERT_QUERY.Render(Hash.FromAnonymousObject(new
            {
                TableName = tableName,
                Schema = schemaName,
                IsIdentityInsert = identityInsert,
                Columns = columns,
                Values = values,
                Where = condition
            }));
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

            bool _isMultiLine = comment.Contains(Environment.NewLine);

            return _template.COMMENT_QUERY.Render(Hash.FromAnonymousObject(new
            {
                isMultiLine = _isMultiLine,
                Comment = comment
            }));
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
        public List<string> GetCondition<T>(T entity, List<string> keyColumns) where T : IDataContractComparer
        {
            Type entityType = typeof(T);
            PropertyInfo[] keyProperties = entityType.GetProperties().
                Where(p => keyColumns.Contains(p.Name)).ToArray();

            return keyProperties.Select(p => $"{EscapeColumn(p.Name)} = '{EscapeValue(p.GetValue(entity))}'").ToList();
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
