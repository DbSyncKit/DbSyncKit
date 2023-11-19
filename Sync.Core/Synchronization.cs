using Sync.Core.Comparer;
using Sync.Core.DataContract;
using Sync.Core.Helper;
using Sync.DB;
using Sync.DB.Helper;
using Sync.DB.Interface;
using System.Text;

namespace Sync.Core
{
    public class Synchronization : QueryHelper
    {
        //private readonly DatabaseMetadata dbSchema;
        private readonly QueryGenerationManager queryGenerationManager;
        public Synchronization(IQueryGenerator querryGenerator)
        {
            //dbSchema = new DatabaseMetadata();
            queryGenerationManager = new QueryGenerationManager(querryGenerator);
        }

        public Result<T> SyncData<T>(IDatabase source, IDatabase destination) where T : IDataContractComparer
        {
            string tableName = GetTableName<T>();

            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(tableName, "Table Name Cannot be null");
            List<string> excludedProperty = GetExcludedProperties<T>();

            List<string> sourceColList = GetAllColumns<T>().Except(excludedProperty).ToList();
            List<string> destinationColList = GetAllColumns<T>().Except(excludedProperty).ToList();

            var sourceList = GetDataFromDatabase<T>(tableName, source, sourceColList);
            var destinationList = GetDataFromDatabase<T>(tableName, destination, destinationColList);

            return DataMetadataComparisonHelper<T>.GetDifferences(sourceList, destinationList, GetKeyColumns<T>(), GetExcludedProperties<T>());

        }

        public string GetSqlQueryForSyncData<T>(Result<T> result) where T : IDataContractComparer
        {
            var inserts = new StringBuilder();
            foreach (var entity in result.Added)
            {
                inserts.AppendLine(queryGenerationManager.GenerateInsertQuery(entity, GetKeyColumns<T>(), GetExcludedProperties<T>()));
            }

            var delete = new StringBuilder();
            foreach (var entity in result.Deleted)
            {
                delete.AppendLine(queryGenerationManager.GenerateDeleteQuery(entity, GetKeyColumns<T>()));
            }

            var edits = new StringBuilder();

            foreach (var (entity, updatedProperties) in result.Edited)
            {
                edits.AppendLine(queryGenerationManager.GenerateUpdateQuery<T>(entity, GetKeyColumns<T>(), GetExcludedProperties<T>(), updatedProperties));
            }


            var query = new StringBuilder();

            var TableName = GetTableName<T>();

            query.AppendLine(queryGenerationManager.GenerateComment("==============" + TableName + "=============="));
            query.AppendLine(queryGenerationManager.GenerateComment("==============Insert==============="));
            query.AppendLine(inserts.ToString());
            query.AppendLine(queryGenerationManager.GenerateComment("==============Delete==============="));
            query.AppendLine(delete.ToString());
            query.AppendLine(queryGenerationManager.GenerateComment("==============Update==============="));
            query.AppendLine(edits.ToString());

            return query.ToString();
        }

        private HashSet<T> GetDataFromDatabase<T>(string tableName, IDatabase connection, List<string> columns) where T : IDataContractComparer
        {
            var query = queryGenerationManager.GenerateSelectQuery<T>(tableName, columns, string.Empty);

            using (var DBManager = new DatabaseManager<IDatabase>(connection))
            {
                return DBManager.ExecuteQuery<T>(query, tableName).ToHashSet(new KeyEqualityComparer<T>(GetKeyColumns<T>(), GetExcludedProperties<T>()));
            }
        }
    }
}
