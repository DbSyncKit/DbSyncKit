using DbSyncKit.Core;
using DbSyncKit.Core.Comparer;
using DbSyncKit.Core.DataContract;
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
        private object sourceList, destinationList;
        private string _tableName;
        private object result;

        public void Setup<T>(Synchronization Sync) where T : IDataContractComparer
        {
            excludedProperty = Sync.GetExcludedColumns<T>();
            ColumnList = Sync.GetAllColumns<T>().Except(excludedProperty).ToList();
            ComparableProperties = Sync.GetComparableProperties<T>();
            keyProperties = Sync.GetKeyProperties<T>();
            keyEqualityComparer = new KeyEqualityComparer<T>(ComparableProperties, keyProperties);
            _tableName = Sync.GetTableName<T>();
        }

        public void GetData<T>(Synchronization Sync, IDatabase Source, IDatabase Destination) where T : IDataContractComparer
        {
            Sync.RetrieveDataFromDatabases<T>(Source, Destination, _tableName, ColumnList, (KeyEqualityComparer<T>)keyEqualityComparer, out HashSet<T> SourceList, out HashSet<T> DestinationList);

            sourceList = SourceList;
            destinationList = DestinationList;
        }

        public void Compare<T>(Synchronization Sync) where T : IDataContractComparer
        {
            result = Sync.GetDifferences<T>((HashSet<T>)sourceList, (HashSet<T>)destinationList, (KeyEqualityComparer<T>)keyEqualityComparer);
        }

        public void GetSqlQueryForSyncData<T>(Synchronization Sync) where T : IDataContractComparer
        {
            Sync.GetSqlQueryForSyncData<T>((Result<T>)result);
        }

        public void CleanUp<T>() where T : IDataContractComparer
        {
            CacheManager.DisposeType(typeof(T));
        }
    }
}
