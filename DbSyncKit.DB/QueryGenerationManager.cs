using DbSyncKit.DB.Interface;

namespace DbSyncKit.DB
{
    /// <summary>
    /// Manages the generation of SQL queries for data operations by delegating the query generation tasks
    /// to an implementation of the <see cref="IQueryGenerator"/> interface.
    /// </summary>
    /// <remarks>
    /// This class acts as a wrapper around an instance of <see cref="IQueryGenerator"/> and forwards
    /// query generation requests to the underlying implementation.
    /// </remarks>
    public class QueryGenerationManager : IQueryGenerator
    {
        private readonly IQueryGenerator _querryGenerator;

        /// <summary>
        /// The underlying query generator instance used for actual query generation.
        /// </summary>
        public QueryGenerationManager(IQueryGenerator querryGenerator)
        {
            _querryGenerator = querryGenerator;
        }

        #region Public Methods
        /// <inheritdoc />
        public string GenerateSelectQuery<T>(string tableName, List<string> listOfColumns, string schemaName) where T : IDataContractComparer
        {
            return _querryGenerator.GenerateSelectQuery<T>(tableName, listOfColumns, schemaName);
        }

        /// <inheritdoc />
        public string GenerateUpdateQuery<T>(T DataContract, List<string> keyColumns, List<string> excludedColumns, Dictionary<string, object> editedProperties) where T : IDataContractComparer
        {
            return _querryGenerator.GenerateUpdateQuery<T>(DataContract, keyColumns, excludedColumns, editedProperties);
        }

        /// <inheritdoc />
        public string GenerateDeleteQuery<T>(T entity, List<string> keyColumns) where T : IDataContractComparer
        {
            return _querryGenerator.GenerateDeleteQuery<T>(entity, keyColumns);
        }

        /// <inheritdoc />
        public string GenerateInsertQuery<T>(T entity, List<string> keyColumns, List<string> excludedColumns) where T : IDataContractComparer
        {
            return _querryGenerator.GenerateInsertQuery<T>(entity, keyColumns, excludedColumns);
        }

        /// <inheritdoc />
        public string GenerateComment(string comment)
        {
            return _querryGenerator.GenerateComment(comment);
        }

        /// <inheritdoc />
        public List<string> GetCondition<T>(T entity, List<string> keyColumns) where T : IDataContractComparer
        {
            return _querryGenerator.GetCondition<T>(entity, keyColumns);
        }

        /// <inheritdoc />
        public object? EscapeValue(object? input)
        {
            return _querryGenerator.EscapeValue(input);
        }

        /// <inheritdoc />
        public string EscapeColumn(string? input)
        {
            return _querryGenerator.EscapeColumn(input);
        }

        /// <inheritdoc />
        public string GenerateBatchSeparator()
        {
            return _querryGenerator.GenerateBatchSeparator();
        }

        #endregion
    }
}
