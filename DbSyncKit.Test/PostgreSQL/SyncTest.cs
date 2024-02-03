using System.Diagnostics;
using DbSyncKit.Core;
using DbSyncKit.Core.Comparer;
using DbSyncKit.Core.DataContract;
using DbSyncKit.DB.Interface;
using DbSyncKit.PostgreSQL;
using DbSyncKit.Test.SampleContract.DataContract;

namespace DbSyncKit.Test.PostgreSQL
{
    [TestClass]
    public class SyncTest
    {
        public IDatabase Source { get; set; }
        public IDatabase Destination { get; set; }
        public Synchronization Sync { get; set; }
        Stopwatch stopwatch { get; }

        public SyncTest()
        {
            Source = new Connection("localhost", 5432, "sourceChinook", "postgres", "");
            Destination = new Connection("localhost", 5432, "destinationChinook", "postgres", "");
            Sync = new Synchronization();
            stopwatch = new Stopwatch();
        }

        [TestMethod]
        public void ConnectionTest()
        {
            var sourceTest = Source.TestConnection();

            Assert.IsTrue(sourceTest);

            var destinationTest = Destination.TestConnection();

            Assert.IsTrue(destinationTest);

            if (sourceTest == true && destinationTest == true)
                Console.WriteLine($"Connection Test is Successful");
            else
                Console.WriteLine($"Connection Test is not Successful");
        }

        private void DataSync<T>()
        {
            var excludedProperty = Sync.GetExcludedColumns<T>();
            var ColumnList = Sync.GetAllColumns<T>().Except(excludedProperty).ToList();
            var ComparableProperties = Sync.GetComparableProperties<T>();
            var keyProperties = Sync.GetKeyProperties<T>();
            var keyEqualityComparer = new PropertyEqualityComparer<T>(keyProperties);
            var ComparablePropertiesComparer = new PropertyEqualityComparer<T>(ComparableProperties);

            var _tableName = Sync.GetTableName<T>();

            stopwatch.Start();
            Sync.ContractFetcher.RetrieveDataFromDatabases<T>(Source, Destination, _tableName, ColumnList, keyEqualityComparer, out HashSet<T> SourceList, out HashSet<T> DestinationList);
            stopwatch.Stop();
            var getDataSpan = stopwatch.Elapsed;
            stopwatch.Restart();


            stopwatch.Start();
            Result<T> data = Sync.MismatchIdentifier.GetDifferences<T>(SourceList, DestinationList, keyEqualityComparer, ComparablePropertiesComparer);
            stopwatch.Stop();
            Console.WriteLine($"Added: {data.Added.Count} EditedDetailed: {data.EditedDetailed.Count} Deleted: {data.Deleted.Count}");
            Console.WriteLine($"Total Source Data: {data.SourceDataCount}");
            Console.WriteLine($"Total Destination Data: {data.DestinaionDataCount}");
            Console.WriteLine($"Time took to Get Data: {GetFormattedTime(getDataSpan)}");
            Console.WriteLine($"Time took to compare: {GetFormattedTime(stopwatch.Elapsed)}");

            stopwatch.Restart();
            var query = Sync.QueryBuilder.GetSqlQueryForSyncData<T>(data, Sync.ContractFetcher.DestinationQueryGenerationManager!);
            stopwatch.Stop();
            Console.WriteLine($"Time took to Generate Query: {GetFormattedTime(stopwatch.Elapsed)}");

            Console.WriteLine(query);
        }

        private string GetFormattedTime(TimeSpan elapsed)
        {
            if (elapsed.TotalMinutes >= 1)
            {
                return $"{elapsed.TotalMinutes:F2} m";
            }
            else if (elapsed.TotalSeconds >= 1)
            {
                return $"{elapsed.TotalSeconds:F2} s";
            }
            else if (elapsed.TotalMilliseconds >= 1)
            {
                return $"{elapsed.TotalMilliseconds:F2} ms";
            }
            else if (elapsed.TotalMicroseconds >= 1)
            {
                return $"{elapsed.TotalMicroseconds:F2} us";
            }
            else
            {
                return $"{elapsed.TotalNanoseconds:F2} ns";
            }

        }

        [TestMethod]
        public void SyncAlbum()
        {
            DataSync<Album>();
        }

        [TestMethod]
        public void SyncArtist()
        {
            DataSync<Artist>();
        }

        [TestMethod]
        public void SyncCustomer()
        {
            DataSync<Customer>();

        }

        [TestMethod]
        public void SyncEmployee()
        {
            DataSync<Employee>();
        }

        [TestMethod]
        public void SyncGenre()
        {
            DataSync<Genre>();
        }

        [TestMethod]
        public void SyncInvoice()
        {
            DataSync<Invoice>();
        }

        [TestMethod]
        public void SyncInvoiceLine()
        {
            DataSync<InvoiceLine>();
        }

        [TestMethod]
        public void SyncMediaType()
        {
            DataSync<MediaType>();
        }

        [TestMethod]
        public void SyncPlaylist()
        {
            DataSync<Playlist>();
        }

        [TestMethod]
        public void SyncPlaylistTrack()
        {
            DataSync<PlaylistTrack>();
        }

        [TestMethod]
        public void SyncTrack()
        {
            DataSync<Track>();
        }
    }
}
