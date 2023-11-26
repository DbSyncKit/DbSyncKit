using Sync.Core.Comparer;
using Sync.Core.DataContract;
using Sync.Core.Helper;
using Sync.DB;
using Sync.DB.Helper;
using Sync.DB.Interface;
using System.Text;

namespace Sync.Core
{
    /// <summary>
    /// Manages the synchronization of data between source and destination databases.
    /// </summary>
    public class Synchronization : QueryHelper
    {
        #region Decleration

        //private readonly DatabaseMetadata dbSchema;

        /// <summary>
        /// The query generation manager used for generating SQL queries.
        /// </summary>
        private readonly QueryGenerationManager queryGenerationManager;

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Synchronization"/> class.
        /// </summary>
        /// <param name="querryGenerator">The query generator to be used for SQL query generation.</param>
        public Synchronization(IQueryGenerator querryGenerator)
        {
            //dbSchema = new DatabaseMetadata();
            queryGenerationManager = new QueryGenerationManager(querryGenerator);
        }

        #endregion

        #region Public Methods


        /// <summary>
        /// Synchronizes data of a specific type between source and destination databases.
        /// </summary>
        /// <typeparam name="T">The type of entity that implements IDataContractComparer.</typeparam>
        /// <param name="source">The source database.</param>
        /// <param name="destination">The destination database.</param>
        /// <returns>A result object containing the differences between source and destination data.</returns>
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

        /// <summary>
        /// Generates SQL queries for synchronizing data based on the differences identified.
        /// </summary>
        /// <typeparam name="T">The type of entity that implements IDataContractComparer.</typeparam>
        /// <param name="result">The result object containing the differences between source and destination data.</param>
        /// <returns>A string representing the generated SQL queries for synchronization.</returns>
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

        #endregion

        #region Private Methods
        private HashSet<T> GetDataFromDatabase<T>(string tableName, IDatabase connection, List<string> columns) where T : IDataContractComparer
        {
            var query = queryGenerationManager.GenerateSelectQuery<T>(tableName, columns, string.Empty);

            using (var DBManager = new DatabaseManager<IDatabase>(connection))
            {
                return DBManager.ExecuteQuery<T>(query, tableName).ToHashSet(new KeyEqualityComparer<T>(GetKeyColumns<T>(), GetExcludedProperties<T>()));
            }
        }

        #endregion
    }
}
