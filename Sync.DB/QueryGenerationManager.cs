using Sync.DB.Interface;

namespace Sync.DB
{
    public class QueryGenerationManager : IQueryGenerator
    {
        private readonly IQueryGenerator _querryGenerator;

        public QueryGenerationManager(IQueryGenerator querryGenerator)
        {
            _querryGenerator = querryGenerator;
        }

        #region Public Methods
        public string GenerateSelectQuery(string tableName, List<string> listOfColumns, string schemaName)
        {
            return _querryGenerator.GenerateSelectQuery(tableName, listOfColumns, schemaName);
        }

        public string GenerateUpdateQuery<T>(T DataContract, List<string> keyColumns, List<string> excludedColumns, Dictionary<string, object> editedProperties) where T : IDataContractComparer
        {
            return _querryGenerator.GenerateUpdateQuery<T>(DataContract, keyColumns, excludedColumns, editedProperties);
        }

        public string GenerateDeleteQuery<T>(T entity, List<string> keyColumns) where T : IDataContractComparer
        {
            return _querryGenerator.GenerateDeleteQuery<T>(entity, keyColumns);
        }

        public string GenerateInsertQuery<T>(T entity, List<string> keyColumns, List<string> excludedColumns) where T : IDataContractComparer
        {
            return _querryGenerator.GenerateInsertQuery<T>(entity, keyColumns, excludedColumns);
        }

        public string GenerateComment(string comment)
        {
            return _querryGenerator.GenerateComment(comment);
        }

        #endregion
    }
}
