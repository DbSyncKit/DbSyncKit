using BenchmarkDotNet.Attributes;
using DbSyncKit.Core;
using DbSyncKit.DB.Interface;
using DbSyncKit.DB.Manager;
using DbSyncKit.MySQL;
using DbSyncKit.Test.SampleContract.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbSyncKit.Benchmarks
{
    [MemoryDiagnoser]
    public class MySqlBenchmark
    {
        public IDatabase Source { get; set; }
        public IDatabase Destination { get; set; }
        public Synchronization Sync { get; set; }

        [GlobalSetup]
        public void MSSQLBenchmarkSetup()
        {
            Source = new Connection("localhost", 3306, "SourceChinook", "root", "");
            Destination = new Connection("localhost", 3306, "DestinationChinook", "root", "");
            Sync = new Synchronization();
        }

        private void DataSync<T>() where T : IDataContractComparer
        {
            var data = Sync.SyncData<T>(Source, Destination);
            Sync.GetSqlQueryForSyncData(data);

            CacheManager.DisposeType(typeof(T));
        }

        public void ConnectionTest()
        {
            var sourceTest = Source.TestConnection();


            var destinationTest = Destination.TestConnection();


            if (sourceTest == true && destinationTest == true)
                Console.WriteLine($"Connection Test is Successful");
            else
                Console.WriteLine($"Connection Test is not Successful");
        }

        [Benchmark]
        public void SyncAlbum()
        {
            DataSync<Album>();
        }

        [Benchmark]
        public void SyncArtist()
        {
            DataSync<Artist>();
        }

        [Benchmark]
        public void SyncCustomer()
        {
            DataSync<Customer>();

        }

        [Benchmark]
        public void SyncEmployee()
        {
            DataSync<Employee>();
        }

        [Benchmark]
        public void SyncGenre()
        {
            DataSync<Genre>();
        }

        [Benchmark]
        public void SyncInvoice()
        {
            DataSync<Invoice>();
        }

        [Benchmark]
        public void SyncInvoiceLine()
        {
            DataSync<InvoiceLine>();
        }

        [Benchmark]
        public void SyncMediaType()
        {
            DataSync<MediaType>();
        }

        [Benchmark]
        public void SyncPlaylist()
        {
            DataSync<Playlist>();
        }

        [Benchmark]
        public void SyncPlaylistTrack()
        {
            DataSync<PlaylistTrack>();
        }

        [Benchmark]
        public void SyncTrack()
        {
            DataSync<Track>();
        }
    }
}
