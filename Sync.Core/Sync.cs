using System.Reflection;
using Sync.Core.Comparer;
using Sync.Core.DataContract;
using Sync.Core.Helper;
using Sync.DB;
using Sync.DB.Attributes;
using Sync.DB.Interface;
using Sync.DB.Utils;

namespace Sync.Core
{
    public class Sync
    {
        private readonly IDatabase Source;
        private readonly IDatabase Destination;
        //private readonly DatabaseMetadata dbSchema;
        private readonly QueryGenerationManager queryGenerationManager;
        public Sync(IDatabase source, IDatabase destination,IQuerryGenerator querryGenerator)
        {
            Source = source;
            Destination = destination;
            //dbSchema = new DatabaseMetadata();
            queryGenerationManager = new QueryGenerationManager(querryGenerator);
        }

        public Result<T> SyncData<T>() where T : IDataContractComparer
        {
            string tableName = typeof(T).Assembly.GetName().Name!;

            if(string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(tableName,"Table Name Cannot be null");

            List<string> sourceColList = new List<string>();
            List<string> destinationColList = new List<string>();

            var sourceList = GetDataFromDatabase<T>(tableName, Source,sourceColList);
            var destinationList = GetDataFromDatabase<T>(tableName, Destination, destinationColList);

            return DataMetadataComparisonHelper<T>.GetDifferences(sourceList,destinationList,GetKeyColumns<T>(),GetExcludedProperties<T>());

        }

        private List<string> GetKeyColumns<T>()
        {
            return typeof(T).GetProperties()
                .Where(prop => !Attribute.IsDefined(prop, typeof(KeyPropertyAttribute))).Select(prop => prop.Name).ToList();
        }

        private List<string> GetExcludedProperties<T>()
        {
            return typeof(T).GetProperties()
               .Where(prop => !Attribute.IsDefined(prop, typeof(ExcludedPropertyAttribute))).Select(prop => prop.Name).ToList();
        }

        private HashSet<T> GetDataFromDatabase<T>(string tableName, IDatabase connection, List<string> columns) where T : IDataContractComparer
        {
            columns = columns
                .Where(prop => !GetExcludedProperties<T>().Contains(prop)).Select(col => $"[{col}]").ToList();
            var querry = $" SELECT {string.Join(",", columns)} FROM {tableName} ";

            var query = queryGenerationManager.GenerateSelectQuery(tableName, columns,string.Empty);

            using (var DBManager = new DatabaseManager<IDatabase>(connection))
            {
                return DBManager.ExecuteQuery<T>(querry, tableName).ToHashSet(new KeyEqualityComparer<T>(GetKeyColumns<T>(), GetExcludedProperties<T>()));
            }
        }

        //private void ProcessDifferences<T>(Result<T> result, Type contractType, ref Dictionary<string, long> changesCounter)
        //{
        //    // Your logic to process added, deleted, and edited entities
        //    Console.WriteLine($"Processing differences for type: {contractType.Name}");

        //    Console.WriteLine("Added:");
        //    Type Type = typeof(T);

        //    var tableName = Type.Name;
        //    var withID = SyncConfiguation.Tables.TryGetValue(tableName, out var table);
        //    if (table == null)
        //    {
        //        return;
        //    }

        //    var inserts = new StringBuilder();
        //    foreach (var entity in result.Added)
        //    {
        //        inserts.AppendLine(queryGenerater.GenerateInsertQuery(entity, GetKeyColumns(tableName), GetExcludedProperties(tableName), table.WithIdentityInsert));
        //    }
        //    Console.WriteLine(inserts.ToString());



        //    Console.WriteLine("\nDeleted:");
        //    var delete = new StringBuilder();
        //    foreach (var entity in result.Deleted)
        //    {
        //        delete.AppendLine(queryGenerater.GenerateDeleteQuery(entity, GetKeyColumns(tableName)));
        //    }

        //    Console.WriteLine(delete.ToString());
        //    var edits = new StringBuilder();


        //    Console.WriteLine("\nEdited:");
        //    foreach (var (entity, updatedProperties) in result.Edited)
        //    {
        //        edits.AppendLine(queryGenerater.GenerateUpdateQuery(entity, GetKeyColumns(tableName), GetExcludedProperties(tableName), updatedProperties));
        //        //Console.WriteLine($"Entity: {entity}, Updated Properties: {string.Join(", ", updatedProperties)}");
        //    }

        //    Console.WriteLine(edits.ToString());


        //    changesCounter["Added"] += result.Added.Count;
        //    changesCounter["Edited"] += result.Edited.Count;
        //    changesCounter["Deleted"] += result.Deleted.Count;
        //}
    }
}
