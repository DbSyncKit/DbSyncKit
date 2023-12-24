using DbSyncKit.Core.Comparer;
using DbSyncKit.Core.DataContract;
using DbSyncKit.Core.Enum;
using DbSyncKit.Core.Helper;
using DbSyncKit.DB;
using DbSyncKit.DB.Factory;
using DbSyncKit.DB.Helper;
using DbSyncKit.DB.Interface;
using System.Text;

namespace DbSyncKit.Core
{
    /// <summary>
    /// Manages the synchronization of data between source and destination databases.
    /// </summary>
    public class Synchronization : QueryHelper
    {
        #region Decleration

        /// <summary>
        /// Gets or sets the IQueryGenerator instance for the destination database.
        /// </summary>
        public IQueryGenerator destinationQueryGenerationManager { get; private set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Synchronization"/> class.
        /// </summary>
        public Synchronization()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Synchronization"/> class with specified IQueryGenerator instances.
        /// </summary>
        /// <param name="destination">The IQueryGenerator for the destination database.</param>
        public Synchronization(IQueryGenerator destination)
        {
            destinationQueryGenerationManager = destination;
        }

        #endregion

        #region Public Methods


        /// <summary>
        /// Synchronizes data of a specific type between source and destination databases.
        /// </summary>
        /// <typeparam name="T">The type of entity that implements IDataContractComparer.</typeparam>
        /// <param name="source">The source database.</param>
        /// <param name="destination">The destination database.</param>
        /// <param name="direction">Represents Which Direction to compare db</param>
        /// <returns>A result object containing the differences between source and destination data.</returns>
        public Result<T> SyncData<T>(IDatabase source, IDatabase destination, Direction direction = Direction.SourceToDestination) where T : IDataContractComparer
        {
            string tableName = GetTableName<T>();

            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(tableName, "Table Name Cannot be null");
            List<string> excludedProperty = GetExcludedProperties<T>();

            List<string> sourceColList = GetAllColumns<T>().Except(excludedProperty).ToList();
            List<string> destinationColList = GetAllColumns<T>().Except(excludedProperty).ToList();
            HashSet<T> sourceList, destinationList;

            switch (direction)
            {
                case Direction.SourceToDestination:
                    break;

                case Direction.DestinationToSource:
                    IDatabase temp;
                    temp = source;
                    source = destination;
                    destination = temp;
                    break;

                case Direction.BiDirectional:
                    throw new NotImplementedException();
                default:
                    break;
            }

            var sourceQueryGenerationManager = new QueryGenerationManager(QueryGeneratorFactory.GetQueryGenerator(source.Provider));
                sourceList = GetDataFromDatabase<T>(tableName, source, sourceQueryGenerationManager, sourceColList);

            if (source.Provider != destination.Provider)
            {
                sourceQueryGenerationManager.Dispose();
                destinationQueryGenerationManager = new QueryGenerationManager(QueryGeneratorFactory.GetQueryGenerator(destination.Provider));
            }
            else
            {
                destinationQueryGenerationManager = sourceQueryGenerationManager;
            }

            destinationList = GetDataFromDatabase<T>(tableName, destination, destinationQueryGenerationManager, destinationColList);

            return DataMetadataComparisonHelper<T>.GetDifferences(sourceList, destinationList, GetKeyColumns<T>(), GetExcludedProperties<T>(), direction);
        }

        /// <summary>
        /// Generates SQL queries for synchronizing data based on the differences identified.
        /// </summary>
        /// <typeparam name="T">The type of entity that implements IDataContractComparer.</typeparam>
        /// <param name="result">The result object containing the differences between source and destination data.</param>
        /// <param name="BatchSize">The size of each batch for SQL statements (default is 20).</param>
        /// <returns>A string representing the generated SQL queries for synchronization.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the IQueryGenerator instance is missing.
        /// Make sure to set it either by providing it through the constructor or by calling
        /// the SyncData method before calling this method.
        /// </exception>
        public string GetSqlQueryForSyncData<T>(Result<T> result,int BatchSize = 20) where T : IDataContractComparer
        {
            if (destinationQueryGenerationManager == null)
            {
                throw new InvalidOperationException(
                    $"The IQueryGenerator instance is missing. " +
                    $"Make sure to set it either by providing it through the constructor or by calling the SyncData method before calling this method.");
            }

            string batchStatement = destinationQueryGenerationManager.GenerateBatchSeparator();
            var inserts = new StringBuilder();
            for (int i = 0; i < result.Added.Count; i++)
            {
                inserts.AppendLine(destinationQueryGenerationManager.GenerateInsertQuery(result.Added[i], GetKeyColumns<T>(), GetExcludedProperties<T>()));
                if (i != 0 && i % BatchSize == 0)
                    inserts.AppendLine(batchStatement);
            }


            var delete = new StringBuilder();
            for (int i = 0; i < result.Deleted.Count; i++)
            {
                delete.AppendLine(destinationQueryGenerationManager.GenerateDeleteQuery(result.Deleted[i], GetKeyColumns<T>()));
                if (i != 0 && i % BatchSize == 0)
                    delete.AppendLine(batchStatement);

            }

            var edits = new StringBuilder();

            for (int i = 0; i < result.Edited.Count; i++)
            {
                edits.AppendLine(destinationQueryGenerationManager.GenerateUpdateQuery<T>(result.Edited[i].Item1, GetKeyColumns<T>(), GetExcludedProperties<T>(), result.Edited[i].Item2));
                if (i != 0 && i % BatchSize == 0)
                    edits.AppendLine(batchStatement);
            }

            var query = new StringBuilder();

            var TableName = GetTableName<T>();

            query.AppendLine(destinationQueryGenerationManager.GenerateComment("==============" + TableName + "=============="));
            query.AppendLine(destinationQueryGenerationManager.GenerateComment("==============Insert==============="));
            query.AppendLine(inserts.ToString());
            if(result.Added.Count > 0 && result.Added.Count % BatchSize != 0) query.AppendLine(batchStatement);
            query.AppendLine(destinationQueryGenerationManager.GenerateComment("==============Delete==============="));
            query.AppendLine(delete.ToString());
            if (result.Deleted.Count > 0 && result.Deleted.Count % BatchSize != 0) query.AppendLine(batchStatement);
            query.AppendLine(destinationQueryGenerationManager.GenerateComment("==============Update==============="));
            query.AppendLine(edits.ToString());
            if (result.Edited.Count > 0 && result.Edited.Count % BatchSize != 0) query.AppendLine(batchStatement);

            return query.ToString();
        }

        #endregion

        #region Private Methods
        private HashSet<T> GetDataFromDatabase<T>(string tableName, IDatabase connection, IQueryGenerator manager, List<string> columns) where T : IDataContractComparer
        {
            var query = manager.GenerateSelectQuery<T>(tableName, columns, string.Empty);

            using (var DBManager = new DatabaseManager<IDatabase>(connection))
            {
                return DBManager.ExecuteQuery<T>(query, tableName).ToHashSet(new KeyEqualityComparer<T>(GetKeyColumns<T>(), GetExcludedProperties<T>()));
            }
        }

        #endregion
    }
}
