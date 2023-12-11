using DbSyncKit.Core;
using DbSyncKit.DB.Interface;
using DbSyncKit.MSSQL;
using DbSyncKit.Test.SampleContract.DataContract;


namespace DbSyncKit.Test.MSSQL
{
    [TestClass]
    public class SyncTest
    {

        public IDatabase Source { get; set; }
        public IDatabase Destination { get; set; }
        public Synchronization Sync { get; set; }

        public SyncTest()
        {
            Source = new Connection("(localdb)\\MSSQLLocalDB", "SourceChinook", true);
            Destination = new Connection("(localdb)\\MSSQLLocalDB", "DestinationChinook", true);
            Sync = new Synchronization(new QueryGenerator());
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

        [TestMethod]
        public void SyncAlbum()
        {
            var album = Sync.SyncData<Album>(Source, Destination);
            Console.WriteLine($"Added: {album.Added.Count} Edited: {album.Edited.Count} Deleted: {album.Deleted.Count}");
            Console.WriteLine($"Total Source Data: {album.SourceDataCount}");
            Console.WriteLine($"Total Destination Data: {album.DestinaionDataCount}");

            var query = Sync.GetSqlQueryForSyncData(album);
            Console.WriteLine(query);
        }

        [TestMethod]
        public void SyncArtist()
        {
            var Artist = Sync.SyncData<Artist>(Source, Destination);
            Console.WriteLine($"Added: {Artist.Added.Count} Edited: {Artist.Edited.Count} Deleted: {Artist.Deleted.Count}");
            Console.WriteLine($"Total Source Data: {Artist.SourceDataCount}");
            Console.WriteLine($"Total Destination Data: {Artist.DestinaionDataCount}");


            var query = Sync.GetSqlQueryForSyncData(Artist);
            Console.WriteLine(query);
        }

        [TestMethod]
        public void SyncCustomer()
        {
            var Customer = Sync.SyncData<Customer>(Source, Destination);
            Console.WriteLine($"Added: {Customer.Added.Count} Edited: {Customer.Edited.Count} Deleted: {Customer.Deleted.Count}");
            Console.WriteLine($"Total Source Data: {Customer.SourceDataCount}");
            Console.WriteLine($"Total Destination Data: {Customer.DestinaionDataCount}");

            var query = Sync.GetSqlQueryForSyncData(Customer);
            Console.WriteLine(query);
        }

        [TestMethod]
        public void SyncEmployee()
        {
            var Employee = Sync.SyncData<Employee>(Source, Destination);
            Console.WriteLine($"Added: {Employee.Added.Count} Edited: {Employee.Edited.Count} Deleted: {Employee.Deleted.Count}");
            Console.WriteLine($"Total Source Data: {Employee.SourceDataCount}");
            Console.WriteLine($"Total Destination Data: {Employee.DestinaionDataCount}");

            var query = Sync.GetSqlQueryForSyncData(Employee);
            Console.WriteLine(query);
        }

        [TestMethod]
        public void SyncGenre()
        {
            var Genre = Sync.SyncData<Genre>(Source, Destination);
            Console.WriteLine($"Added: {Genre.Added.Count} Edited: {Genre.Edited.Count} Deleted: {Genre.Deleted.Count}");
            Console.WriteLine($"Total Source Data: {Genre.SourceDataCount}");
            Console.WriteLine($"Total Destination Data: {Genre.DestinaionDataCount}");

            var query = Sync.GetSqlQueryForSyncData(Genre);
            Console.WriteLine(query);
        }

        [TestMethod]
        public void SyncInvoice()
        {
            var Invoice = Sync.SyncData<Invoice>(Source, Destination);
            Console.WriteLine($"Added: {Invoice.Added.Count} Edited: {Invoice.Edited.Count} Deleted: {Invoice.Deleted.Count}");
            Console.WriteLine($"Total Source Data: {Invoice.SourceDataCount}");
            Console.WriteLine($"Total Destination Data: {Invoice.DestinaionDataCount}");

            var query = Sync.GetSqlQueryForSyncData(Invoice);
            Console.WriteLine(query);
        }

        [TestMethod]
        public void SyncInvoiceLine()
        {
            var InvoiceLine = Sync.SyncData<InvoiceLine>(Source, Destination);
            Console.WriteLine($"Added: {InvoiceLine.Added.Count} Edited: {InvoiceLine.Edited.Count} Deleted: {InvoiceLine.Deleted.Count}");
            Console.WriteLine($"Total Source Data: {InvoiceLine.SourceDataCount}");
            Console.WriteLine($"Total Destination Data: {InvoiceLine.DestinaionDataCount}");

            var query = Sync.GetSqlQueryForSyncData(InvoiceLine);
            Console.WriteLine(query);
        }

        [TestMethod]
        public void SyncMediaType()
        {
            var MediaType = Sync.SyncData<MediaType>(Source, Destination);
            Console.WriteLine($"Added: {MediaType.Added.Count} Edited: {MediaType.Edited.Count} Deleted: {MediaType.Deleted.Count}");
            Console.WriteLine($"Total Source Data: {MediaType.SourceDataCount}");
            Console.WriteLine($"Total Destination Data: {MediaType.DestinaionDataCount}");

            var query = Sync.GetSqlQueryForSyncData(MediaType);
            Console.WriteLine(query);
        }

        [TestMethod]
        public void SyncPlaylist()
        {
            var Playlist = Sync.SyncData<Playlist>(Source, Destination);
            Console.WriteLine($"Added: {Playlist.Added.Count} Edited: {Playlist.Edited.Count} Deleted: {Playlist.Deleted.Count}");
            Console.WriteLine($"Total Source Data: {Playlist.SourceDataCount}");
            Console.WriteLine($"Total Destination Data: {Playlist.DestinaionDataCount}");

            var query = Sync.GetSqlQueryForSyncData(Playlist);
            Console.WriteLine(query);
        }

        [TestMethod]
        public void SyncPlaylistTrack()
        {
            var PlaylistTrack = Sync.SyncData<PlaylistTrack>(Source, Destination);
            Console.WriteLine($"Added: {PlaylistTrack.Added.Count} Edited: {PlaylistTrack.Edited.Count} Deleted: {PlaylistTrack.Deleted.Count}");
            Console.WriteLine($"Total Source Data: {PlaylistTrack.SourceDataCount}");
            Console.WriteLine($"Total Destination Data: {PlaylistTrack.DestinaionDataCount}");

            var query = Sync.GetSqlQueryForSyncData(PlaylistTrack);
            Console.WriteLine(query);
        }

        [TestMethod]
        public void SyncTrack()
        {
            var Track = Sync.SyncData<Track>(Source, Destination);
            Console.WriteLine($"Added: {Track.Added.Count} Edited: {Track.Edited.Count} Deleted: {Track.Deleted.Count}");
            Console.WriteLine($"Total Source Data: {Track.SourceDataCount}");
            Console.WriteLine($"Total Destination Data: {Track.DestinaionDataCount}");

            var query = Sync.GetSqlQueryForSyncData(Track);
            Console.WriteLine(query);
        }
    }
}
