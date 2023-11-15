using Sync.Core.Comparer;
using Sync.Core.DataContract;
using Sync.Core.DataContract.Config;
using Sync.Core.Helper;
using Sync.DB;
using Sync.DB.Interface;
using Sync.DB.Utils;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sync.Core
{
    public class Sync
    {
        private readonly IDatabase Source;
        private readonly IDatabase Destination;
        //private readonly DatabaseMetadata dbSchema;
        //private readonly QueryGenerator queryGenerater;
        public Sync(IDatabase source, IDatabase destination)
        {
            Source = source;
            Destination = destination;
            //dbSchema = new DatabaseMetadata();
            //queryGenerater = new QueryGenerator();
        }
        public void SyncTables(List<string> tableNames)
        {
            Dictionary<string, long> changesCouter = new();
            changesCouter.Add("Added", 0);
            changesCouter.Add("Deleted", 0);
            changesCouter.Add("Edited", 0);

            foreach (var tableName in tableNames)
            {
                // Assuming contract class is in the same namespace and follows the naming convention
                var contractTypeName = SyncConfiguation.DataContractList.FirstOrDefault(contract => contract.Key == tableName).Value;

                // Use reflection to get the contract type
                Type contractType = Type.GetType(contractTypeName);

                if (contractType == null)
                {
                    Console.WriteLine($"Contract not found for table {tableName}");
                    continue;
                }

                // Use reflection to get the list of entities from the database for the current table
                var methodInfo = this.GetType().GetMethod("GetDataFromDatabase", BindingFlags.NonPublic | BindingFlags.Instance);
                var genericMethod = methodInfo.MakeGenericMethod(contractType);

                //List<string> sourceColList = dbSchema.GetColumns(Source, tableName).Select(col => col.column_name).ToList();
                //List<string> destinationColList = dbSchema.GetColumns(Destination, tableName).Select(col => col.column_name).ToList();
                List<string> sourceColList = new List<string>();
                List<string> destinationColList = new List<string>();
                var sourceList = (IEnumerable<object>)genericMethod.Invoke(this, new object[] { tableName, Source, sourceColList });

                // Use reflection to get the list of entities from the destination
                var destinationList = (IEnumerable<object>)genericMethod.Invoke(this, new object[] { tableName, Destination, destinationColList });

                // Use the MetadataComparer to get the differences
                var comparerType = typeof(DataMetadataComparisonHelper<>).MakeGenericType(contractType);
                var comparer = Activator.CreateInstance(comparerType);

                // Get the MethodInfo for the GetDifferences method
                var getDifferencesMethodInfo = comparerType.GetMethod("GetDifferences", BindingFlags.Public | BindingFlags.Static);

                // Invoke the method on the current instance (this)
                var result = getDifferencesMethodInfo.Invoke(null, new object[] { sourceList, destinationList, GetKeyColumns(tableName), GetExcludedProperties(tableName) });

                // Process the differences
                //ProcessDifferences((dynamic)result, contractType, ref changesCouter);

            }

            Console.WriteLine($"Edits: {changesCouter["Edited"]} Added: {changesCouter["Added"]} Deleted: {changesCouter["Deleted"]}");
        }

        private List<string> GetExcludedProperties(string tableName)
        {
            return SyncConfiguation.GetExcludedList(tableName);
        }

        private List<string> GetKeyColumns(string tableName)
        {
            return SyncConfiguation.GetKeyColumnsList(tableName);
        }

        private HashSet<T> GetDataFromDatabase<T>(string tableName, IDatabase connection, List<string> columns) where T : DataContractUtility<T>
        {
            columns = columns
                .Where(prop => !GetExcludedProperties(tableName).Contains(prop)).Select(col => $"[{col}]").ToList();
            var querry = $"SELECT {string.Join(",", columns)} FROM {tableName}";

            using (var DBManager = new DatabaseManager<IDatabase>(connection))
            {
                return DBManager.ExecuteQuery<T>(querry, tableName).ToHashSet(new KeyEqualityComparer<T>(GetKeyColumns(tableName), GetExcludedProperties(tableName)));
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
