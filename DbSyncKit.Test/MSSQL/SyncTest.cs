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

        private void DataSync<T>() where T : IDataContractComparer
        {
            var data = Sync.SyncData<T>(Source, Destination);
            Console.WriteLine($"Added: {data.Added.Count} Edited: {data.Edited.Count} Deleted: {data.Deleted.Count}");
            Console.WriteLine($"Total Source Data: {data.SourceDataCount}");
            Console.WriteLine($"Total Destination Data: {data.DestinaionDataCount}");

            var query = Sync.GetSqlQueryForSyncData(data);
            Console.WriteLine(query);
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
