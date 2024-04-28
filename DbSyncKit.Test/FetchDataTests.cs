using DbSyncKit.Core;
using DbSyncKit.DB.Comparer;
using DbSyncKit.DB.Interface;
using DbSyncKit.Test.SampleContract.DataContract;
using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbSyncKit.UnitTest
{
    public class FetchDataTests
    {
        protected Synchronization Sync { get; set; }

        [SetUp]
        public void setup()
        {
            Sync = new Synchronization();
        }

        public void FetchData<T>(IDatabase Source, IDatabase Destination)
        {
            var excludedProperty = Sync.GetExcludedColumns<T>();
            var ColumnList = Sync.GetAllColumns<T>().Except(excludedProperty).ToList();
            var keyProperties = Sync.GetKeyProperties<T>();
            var keyEqualityComparer = new PropertyEqualityComparer<T>(keyProperties);

            var _tableName = Sync.GetTableName<T>();

            Sync.ContractFetcher.RetrieveDataFromDatabases<T>(Source, Destination, _tableName, ColumnList, keyEqualityComparer, null, out HashSet<T> SourceList, out HashSet<T> DestinationList);
        }

        [Test]
        public void FetchMssqlData()
        {
            var Source = new MSSQL.Connection("(localdb)\\MSSQLLocalDB", false, "SourceChinook");
            var Destination = new MSSQL.Connection("(localdb)\\MSSQLLocalDB", false, "DestinationChinook");

            FetchData<Album>(Source,Destination);
        }

        [Test]
        public void FetchMySqlData()
        {
            var Source = new MySQL.Connection("localhost", 3306, "SourceChinook", "root", "");
            var Destination = new MySQL.Connection("localhost", 3306, "DestinationChinook", "root", "");

            FetchData<Album>(Source, Destination);

        }
        [Test]
        public void FetchSqliteData()
        {
            SQLitePCL.Batteries.Init();
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var Source = new SQLite.Connection(Path.Combine(baseDirectory, "Data", "sourceChinook.sqlite"));
            var Destination = new SQLite.Connection(Path.Combine(baseDirectory, "Data", "destinationChinook.sqlite"));


            FetchData<Album>(Source, Destination);

        }

        [Test]
        public void FetchPostgresData()
        {
            var Source = new PostgreSQL.Connection("localhost", 5432, "sourceChinook", "postgres", "");
            var Destination = new PostgreSQL.Connection("localhost", 5432, "destinationChinook", "postgres", "");

            FetchData<Album>(Source, Destination);

        }
    }
}
