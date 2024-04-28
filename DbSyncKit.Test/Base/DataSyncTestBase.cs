using DbSyncKit.Core;
using DbSyncKit.Core.DataContract;
using DbSyncKit.DB.Comparer;
using DbSyncKit.DB.Interface;
using DbSyncKit.Test.SampleContract.DataContract;
using System.Diagnostics;

namespace DbSyncKit.UnitTest.Base
{
    public abstract class DataSyncTestBase
    {
        protected IDatabase Source { get; set; }
        protected IDatabase Destination { get; set; }
        protected Synchronization Sync { get; set; }
        protected Stopwatch Stopwatch { get; set; }

        protected DataSyncTestBase()
        {
            Source = GetSourceDatabase();
            Destination = GetDestinationDatabase();
            Sync = new Synchronization();
            Stopwatch = new Stopwatch();
        }

        protected abstract IDatabase GetSourceDatabase();
        protected abstract IDatabase GetDestinationDatabase();

        protected Result<T> DataSync<T>()
        {
            var excludedProperty = Sync.GetExcludedColumns<T>();
            var ColumnList = Sync.GetAllColumns<T>().Except(excludedProperty).ToList();
            var ComparableProperties = Sync.GetComparableProperties<T>();
            var keyProperties = Sync.GetKeyProperties<T>();
            var keyEqualityComparer = new PropertyEqualityComparer<T>(keyProperties);
            var ComparablePropertiesComparer = new PropertyEqualityComparer<T>(ComparableProperties);

            var _tableName = Sync.GetTableName<T>();

            Stopwatch.Start();
            Sync.ContractFetcher.RetrieveDataFromDatabases<T>(Source, Destination, _tableName, ColumnList, keyEqualityComparer, null, out HashSet<T> SourceList, out HashSet<T> DestinationList);
            Stopwatch.Stop();
            var getDataSpan = Stopwatch.Elapsed;
            Stopwatch.Restart();

            Stopwatch.Start();
            Result<T> data = Sync.MismatchIdentifier.GetDifferences<T>(SourceList, DestinationList, keyEqualityComparer, ComparablePropertiesComparer);
            Stopwatch.Stop();
            Console.WriteLine($"Added: {data.Added.Count} EditedDetailed: {data.EditedDetailed.Count} Deleted: {data.Deleted.Count}");
            Console.WriteLine($"Total Source Data: {data.SourceDataCount}");
            Console.WriteLine($"Total Destination Data: {data.DestinaionDataCount}");
            Console.WriteLine($"Time took to Get Data: {GetFormattedTime(getDataSpan)}");
            Console.WriteLine($"Time took to compare: {GetFormattedTime(Stopwatch.Elapsed)}");

            Stopwatch.Restart();
            var query = Sync.QueryBuilder.GetSqlQueryForSyncData<T>(data, Sync.ContractFetcher.DestinationQueryGenerationManager!);
            Stopwatch.Stop();
            Console.WriteLine($"Time took to Generate Query: {GetFormattedTime(Stopwatch.Elapsed)}");

            Console.WriteLine(query);

            return data;
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

        [Test]
        public void SyncAlbum()
        {
            var result = DataSync<Album>();

            Assert.IsNotNull(result);
            Assert.That( result.Added.Count, Is.EqualTo(2));
            Assert.That(result.EditedDetailed.Count, Is.EqualTo(4));
            Assert.That(result.Deleted.Count, Is.EqualTo(0));
            Assert.That(result.SourceDataCount, Is.EqualTo(347));
            Assert.That(result.DestinaionDataCount, Is.EqualTo(345));
        }

        [Test]
        public void SyncArtist()
        {
            var result = DataSync<Artist>();

            Assert.IsNotNull(result);
            Assert.That(result.Added.Count, Is.EqualTo(0));
            Assert.That(result.EditedDetailed.Count, Is.EqualTo(3));
            Assert.That(result.Deleted.Count, Is.EqualTo(0));
            Assert.That(result.SourceDataCount, Is.EqualTo(275));
            Assert.That(result.DestinaionDataCount, Is.EqualTo(275));
        }

        [Test]
        public void SyncCustomer()
        {
            var result = DataSync<Customer>();

            Assert.IsNotNull(result);
            Assert.That(result.Added.Count, Is.EqualTo(1));
            Assert.That(result.EditedDetailed.Count, Is.EqualTo(17));
            Assert.That(result.Deleted.Count, Is.EqualTo(0));
            Assert.That(result.SourceDataCount, Is.EqualTo(59));
            Assert.That(result.DestinaionDataCount, Is.EqualTo(58));

        }

        [Test]
        public void SyncEmployee()
        {
            var result = DataSync<Employee>();

            Assert.IsNotNull(result);
            Assert.That(result.Added.Count, Is.EqualTo(0));
            Assert.That(result.EditedDetailed.Count, Is.EqualTo(3));
            Assert.That(result.Deleted.Count, Is.EqualTo(0));
            Assert.That(result.SourceDataCount, Is.EqualTo(8));
            Assert.That(result.DestinaionDataCount, Is.EqualTo(8));
        }

        [Test]
        public void SyncGenre()
        {
            var result = DataSync<Genre>();

            Assert.IsNotNull(result);
            Assert.That(result.Added.Count, Is.EqualTo(0));
            Assert.That(result.EditedDetailed.Count, Is.EqualTo(2));
            Assert.That(result.Deleted.Count, Is.EqualTo(0));
            Assert.That(result.SourceDataCount, Is.EqualTo(25));
            Assert.That(result.DestinaionDataCount, Is.EqualTo(25));
        }

        [Test]
        public void SyncInvoice()
        {
            var result = DataSync<Invoice>();

            Assert.IsNotNull(result);
            Assert.That(result.Added.Count, Is.EqualTo(6));
            Assert.That(result.EditedDetailed.Count, Is.EqualTo(29));
            Assert.That(result.Deleted.Count, Is.EqualTo(0));
            Assert.That(result.SourceDataCount, Is.EqualTo(412));
            Assert.That(result.DestinaionDataCount, Is.EqualTo(406));
        }

        [Test]
        public void SyncInvoiceLine()
        {
            var result = DataSync<InvoiceLine>();

            Assert.IsNotNull(result);
            Assert.That(result.Added.Count, Is.EqualTo(36));
            Assert.That(result.EditedDetailed.Count, Is.EqualTo(0));
            Assert.That(result.Deleted.Count, Is.EqualTo(1));
            Assert.That(result.SourceDataCount, Is.EqualTo(2239));
            Assert.That(result.DestinaionDataCount, Is.EqualTo(2204));
        }

        [Test]
        public void SyncMediaType()
        {
            var result = DataSync<MediaType>();

            Assert.IsNotNull(result);
            Assert.That(result.Added.Count, Is.EqualTo(0));
            Assert.That(result.EditedDetailed.Count, Is.EqualTo(0));
            Assert.That(result.Deleted.Count, Is.EqualTo(0));
            Assert.That(result.SourceDataCount, Is.EqualTo(5));
            Assert.That(result.DestinaionDataCount, Is.EqualTo(5));
        }

        [Test]
        public void SyncPlaylist()
        {
            var result = DataSync<Playlist>();

            Assert.IsNotNull(result);
            Assert.That(result.Added.Count, Is.EqualTo(0));
            Assert.That(result.EditedDetailed.Count, Is.EqualTo(0));
            Assert.That(result.Deleted.Count, Is.EqualTo(0));
            Assert.That(result.SourceDataCount, Is.EqualTo(18));
            Assert.That(result.DestinaionDataCount, Is.EqualTo(18));
        }

        [Test]
        public void SyncPlaylistTrack()
        {
            var result = DataSync<PlaylistTrack>();

            Assert.IsNotNull(result);
            Assert.That(result.Added.Count, Is.EqualTo(9));
            Assert.That(result.EditedDetailed.Count, Is.EqualTo(0));
            Assert.That(result.Deleted.Count, Is.EqualTo(2));
            Assert.That(result.SourceDataCount, Is.EqualTo(8713));
            Assert.That(result.DestinaionDataCount, Is.EqualTo(8706));
        }

        [Test]
        public void SyncTrack()
        {
            var result = DataSync<Track>();

            Assert.IsNotNull(result);
            Assert.That(result.Added.Count, Is.EqualTo(2));
            Assert.That(result.EditedDetailed.Count, Is.EqualTo(166));
            Assert.That(result.Deleted.Count, Is.EqualTo(1));
            Assert.That(result.SourceDataCount, Is.EqualTo(3502));
            Assert.That(result.DestinaionDataCount, Is.EqualTo(3501));
        }
    }
}
