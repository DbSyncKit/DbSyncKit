using System.Reflection;
using System.Text;

using DbSyncKit.DB.Helper;
using DbSyncKit.DB.Interface;

namespace DbSyncKit.MySQL
{
    public class QueryGenerator : QueryHelper, IQueryGenerator
    {
        #region Declaration
        private StringBuilder queryBuilder;
        private readonly string DEFAULT_SCHEMA_NAME = string.Empty;

        private readonly string DELETE_QUERY = @"
DELETE FROM @SchemaWithTableName WHERE @Where LIMIT 1; ";

        private readonly string UPDATE_QUERY = @" 
UPDATE @SchemaWithTableName SET @Set WHERE @Where LIMIT 1; ";

        private readonly string INSERT_QUERY = @"
INSERT INTO @SchemaWithTableName (@Columns) SELECT @Values FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM @SchemaWithTableName WHERE @Where); ";

        private readonly string SELECT_QUERY = @"
SELECT @Columns FROM @SchemaWithTableName 
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

        public string EscapeColumn(string? input)
        {
            return $"`{input}`";
        }

        public object? EscapeValue(object? input)
        {
            if(input is string)
                return MySql.Data.MySqlClient.MySqlHelper.EscapeString(input as string);

            return input;
        }

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

        public string GenerateUpdateQuery<T>(T DataContract, List<string> keyColumns, List<string> excludedColumns, Dictionary<string, object> editedProperties) where T : IDataContractComparer
        {
            queryBuilder.Clear();

            queryBuilder.AppendLine(UPDATE_QUERY);

            string tableName = GetTableName<T>();
            string schemaName = GetTableSchema<T>() ?? DEFAULT_SCHEMA_NAME;
            string setClause = string.Join(", ", editedProperties.Select(kv => $"{EscapeColumn(kv.Key)} = '{EscapeValue(kv.Value)}'"));
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

        public string GetCondition<T>(T entity, List<string> keyColumns) where T : IDataContractComparer
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Batch Seperator dosent exists
        /// </summary>
        /// <returns>Empty string</returns>

        public string GenerateBatchSeparator()
        {
            return string.Empty;
        }
    }
}
