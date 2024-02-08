using DbSyncKit.Core;
using DbSyncKit.Core.Comparer;
using DbSyncKit.Core.DataContract;
using DbSyncKit.Core.Fetcher;
using DbSyncKit.DB.Interface;
using DbSyncKit.DB.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DbSyncKit.Benchmarks
{
    public class GlobalHelper
    {
        private List<string> excludedProperty;
        private List<string> ColumnList;
        private PropertyInfo[] ComparableProperties;
        private PropertyInfo[] keyProperties;
        private object keyEqualityComparer; 
        private object ComparablePropertiesComparer;
        private object sourceList, destinationList;
        private string _tableName;
        private object result;

        public void Setup<T>(Synchronization Sync)
        {
            excludedProperty = Sync.GetExcludedColumns<T>();
            ColumnList = Sync.GetAllColumns<T>().Except(excludedProperty).ToList();
            ComparableProperties = Sync.GetComparableProperties<T>();
            keyProperties = Sync.GetKeyProperties<T>();
            keyEqualityComparer = new PropertyEqualityComparer<T>(keyProperties);
            ComparablePropertiesComparer = new PropertyEqualityComparer<T>(ComparableProperties);
            _tableName = Sync.GetTableName<T>();
        }

        public void GetData<T>(Synchronization Sync, IDatabase Source, IDatabase Destination, FilterCallback<T>? filterCallback = null)
        {
            Sync.ContractFetcher.RetrieveDataFromDatabases<T>(Source, Destination, _tableName, ColumnList, (PropertyEqualityComparer<T>)ComparablePropertiesComparer, filterCallback, out HashSet<T> SourceList, out HashSet<T> DestinationList);

            sourceList = SourceList;
            destinationList = DestinationList;
        }

        public void Compare<T>(Synchronization Sync)
        {
            result = Sync.MismatchIdentifier.GetDifferences<T>((HashSet<T>)sourceList, (HashSet<T>)destinationList, (PropertyEqualityComparer<T>)keyEqualityComparer, (PropertyEqualityComparer<T>)ComparablePropertiesComparer);
        }

        public void GetSqlQueryForSyncData<T>(Synchronization Sync)
        {
            Sync.QueryBuilder.GetSqlQueryForSyncData<T>((Result<T>)result,Sync.ContractFetcher.DestinationQueryGenerationManager!);
        }

        public void CleanUp<T>()
        {
            CacheManager.DisposeType(typeof(T));
        }
    }
}
