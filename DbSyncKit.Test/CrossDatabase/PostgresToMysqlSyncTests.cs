﻿using DbSyncKit.Core;
using DbSyncKit.Core.DataContract;
using DbSyncKit.DB.Interface;
using DbSyncKit.Test.SampleContract.DataContract;
using System.Diagnostics;

namespace DbSyncKit.Test.CrossDatabase
{
    [TestClass]

    public class PostgresToMysqlSyncTests
    {
        public IDatabase Source { get; set; }
        public IDatabase Destination { get; set; }
        public Synchronization Sync { get; set; }
        Stopwatch stopwatch { get; }

        public PostgresToMysqlSyncTests()
        {
            Source = new DbSyncKit.PostgreSQL.Connection("localhost", 5432, "sourceChinook", "postgres", "");
            Destination = new DbSyncKit.MySQL.Connection("localhost", 3306, "destinationChinook", "root", "");
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
            stopwatch.Start();
            Result<T> data = Sync.SyncData<T>(Source, Destination, null);
            stopwatch.Stop();
            Console.WriteLine($"Added: {data.Added.Count} EditedDetailed: {data.EditedDetailed.Count} Deleted: {data.Deleted.Count}");
            Console.WriteLine($"Total Source Data: {data.SourceDataCount}");
            Console.WriteLine($"Total Destination Data: {data.DestinaionDataCount}");
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
            else
            {
                return $"{elapsed.TotalMilliseconds:F2} ms";
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
