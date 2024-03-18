using BenchmarkDotNet.Attributes;
using DbSyncKit.Core;
using DbSyncKit.DB.Interface;
using DbSyncKit.MySQL;
using DbSyncKit.Test.SampleContract.DataContract;

namespace DbSyncKit.Benchmarks
{
    [MemoryDiagnoser]
    public class SyncCompareDifferenceBenchmark
    {
        public IDatabase Source { get; set; }
        public IDatabase Destination { get; set; }
        public Synchronization Sync { get; set; }
        public GlobalHelper GlobalHelper { get; set; }

        #region Sync Album

        [GlobalSetup(Target = nameof(SyncAlbum))] 
        public void SyncAlbumSetup()
        {
            GlobalHelper = new GlobalHelper();
            Source = new Connection("localhost", 3306, "SourceChinook", "root", "");
            Destination = new Connection("localhost", 3306, "DestinationChinook", "root", "");
            Sync = new Synchronization();

            GlobalHelper.Setup<Album>(Sync);
            GlobalHelper.GetData<Album>(Sync, Source, Destination);
        }

        [Benchmark]
        public void SyncAlbum()
        {
            GlobalHelper.Compare<Album>(Sync);
            //GlobalHelper.GetSqlQueryForSyncData<Album>(Sync);
        }

        [GlobalCleanup(Target = nameof(SyncAlbum))] 
        public void SyncAlbumCleanup() 
        {
            GlobalHelper.CleanUp<Album>();
        }

        #endregion

        #region Sync Artist

        [GlobalSetup(Target = nameof(SyncArtist))]
        public void SyncArtistSetup()
        {
            GlobalHelper = new GlobalHelper();
            Source = new Connection("localhost", 3306, "SourceChinook", "root", "");
            Destination = new Connection("localhost", 3306, "DestinationChinook", "root", "");
            Sync = new Synchronization();

            GlobalHelper.Setup<Artist>(Sync);
            GlobalHelper.GetData<Artist>(Sync, Source, Destination);
        }

        [Benchmark]
        public void SyncArtist()
        {
            GlobalHelper.Compare<Artist>(Sync);
            //GlobalHelper.GetSqlQueryForSyncData<Artist>(Sync);
        }

        [GlobalCleanup(Target = nameof(SyncArtist))]
        public void SyncArtistCleanup()
        {
            GlobalHelper.CleanUp<Artist>();
        }

        #endregion

        #region Sync Customer


        [GlobalSetup(Target = nameof(SyncCustomer))]
        public void SyncCustomerSetup()
        {
            GlobalHelper = new GlobalHelper();
            Source = new Connection("localhost", 3306, "SourceChinook", "root", "");
            Destination = new Connection("localhost", 3306, "DestinationChinook", "root", "");
            Sync = new Synchronization();

            GlobalHelper.Setup<Customer>(Sync);
            GlobalHelper.GetData<Customer>(Sync, Source, Destination);
        }

        [Benchmark]
        public void SyncCustomer()
        {
            GlobalHelper.Compare<Customer>(Sync);
            //GlobalHelper.GetSqlQueryForSyncData<Customer>(Sync);
        }

        [GlobalCleanup(Target = nameof(SyncCustomer))]
        public void SyncCustomerCleanup()
        {
            GlobalHelper.CleanUp<Customer>();
        }

        #endregion


        #region Sync Employee


        [GlobalSetup(Target = nameof(SyncEmployee))]
        public void SyncEmployeeSetup()
        {
            GlobalHelper = new GlobalHelper();
            Source = new Connection("localhost", 3306, "SourceChinook", "root", "");
            Destination = new Connection("localhost", 3306, "DestinationChinook", "root", "");
            Sync = new Synchronization();

            GlobalHelper.Setup<Employee>(Sync);
            GlobalHelper.GetData<Employee>(Sync, Source, Destination);
        }

        [Benchmark]
        public void SyncEmployee()
        {
            GlobalHelper.Compare<Employee>(Sync);
            //GlobalHelper.GetSqlQueryForSyncData<Employee>(Sync);
        }

        [GlobalCleanup(Target = nameof(SyncEmployee))]
        public void SyncEmployeeCleanup()
        {
            GlobalHelper.CleanUp<Employee>();
        }

        #endregion


        #region Sync Genre


        [GlobalSetup(Target = nameof(SyncGenre))]
        public void SyncGenreSetup()
        {
            GlobalHelper = new GlobalHelper();
            Source = new Connection("localhost", 3306, "SourceChinook", "root", "");
            Destination = new Connection("localhost", 3306, "DestinationChinook", "root", "");
            Sync = new Synchronization();

            GlobalHelper.Setup<Genre>(Sync);
            GlobalHelper.GetData<Genre>(Sync, Source, Destination);
        }

        [Benchmark]
        public void SyncGenre()
        {
            GlobalHelper.Compare<Genre>(Sync);
            //GlobalHelper.GetSqlQueryForSyncData<Genre>(Sync);

        }

        [GlobalCleanup(Target = nameof(SyncGenre))]
        public void SyncGenreCleanup()
        {
            GlobalHelper.CleanUp<Genre>();
        }

        #endregion


        #region Sync Invoice


        [GlobalSetup(Target = nameof(SyncInvoice))]
        public void SyncInvoiceSetup()
        {
            GlobalHelper = new GlobalHelper();
            Source = new Connection("localhost", 3306, "SourceChinook", "root", "");
            Destination = new Connection("localhost", 3306, "DestinationChinook", "root", "");
            Sync = new Synchronization();

            GlobalHelper.Setup<Invoice>(Sync);
            GlobalHelper.GetData<Invoice>(Sync, Source, Destination);
        }

        [Benchmark]
        public void SyncInvoice()
        {
            GlobalHelper.Compare<Invoice>(Sync);
            //GlobalHelper.GetSqlQueryForSyncData<Invoice>(Sync);
        }

        [GlobalCleanup(Target = nameof(SyncInvoice))]
        public void SyncInvoiceCleanup()
        {
            GlobalHelper.CleanUp<Invoice>();
        }

        #endregion


        #region Sync Invoice Line


        [GlobalSetup(Target = nameof(SyncInvoiceLine))]
        public void SyncInvoiceLineSetup()
        {
            GlobalHelper = new GlobalHelper();
            Source = new Connection("localhost", 3306, "SourceChinook", "root", "");
            Destination = new Connection("localhost", 3306, "DestinationChinook", "root", "");
            Sync = new Synchronization();

            GlobalHelper.Setup<InvoiceLine>(Sync);
            GlobalHelper.GetData<InvoiceLine>(Sync, Source, Destination);
        }

        [Benchmark]
        public void SyncInvoiceLine()
        {
            GlobalHelper.Compare<InvoiceLine>(Sync);
            //GlobalHelper.GetSqlQueryForSyncData<InvoiceLine>(Sync);

        }

        [GlobalCleanup(Target = nameof(SyncInvoiceLine))]
        public void SyncInvoiceLineCleanup()
        {
            GlobalHelper.CleanUp<InvoiceLine>();
        }

        #endregion

        #region Sync MediaType


        [GlobalSetup(Target = nameof(SyncMediaType))]
        public void SyncMediaTypeSetup()
        {
            GlobalHelper = new GlobalHelper();
            Source = new Connection("localhost", 3306, "SourceChinook", "root", "");
            Destination = new Connection("localhost", 3306, "DestinationChinook", "root", "");
            Sync = new Synchronization();

            GlobalHelper.Setup<MediaType>(Sync);
            GlobalHelper.GetData<MediaType>(Sync, Source, Destination);
        }

        [Benchmark]
        public void SyncMediaType()
        {
            GlobalHelper.Compare<MediaType>(Sync);
            //GlobalHelper.GetSqlQueryForSyncData<MediaType>(Sync);

        }

        [GlobalCleanup(Target = nameof(SyncMediaType))]
        public void SyncMediaTypeCleanup()
        {
            GlobalHelper.CleanUp<MediaType>();
        }

        #endregion

        #region Sync Playlist


        [GlobalSetup(Target = nameof(SyncPlaylist))]
        public void SyncPlaylistSetup()
        {
            GlobalHelper = new GlobalHelper();
            Source = new Connection("localhost", 3306, "SourceChinook", "root", "");
            Destination = new Connection("localhost", 3306, "DestinationChinook", "root", "");
            Sync = new Synchronization();

            GlobalHelper.Setup<Playlist>(Sync);
            GlobalHelper.GetData<Playlist>(Sync, Source, Destination);
        }

        [Benchmark]
        public void SyncPlaylist()
        {
            GlobalHelper.Compare<Playlist>(Sync);
            //GlobalHelper.GetSqlQueryForSyncData<Playlist>(Sync);

        }

        [GlobalCleanup(Target = nameof(SyncPlaylist))]
        public void SyncPlaylistCleanup()
        {
            GlobalHelper.CleanUp<Playlist>();
        }

        #endregion

        #region Sync PlaylistTrack


        [GlobalSetup(Target = nameof(SyncPlaylistTrack))]
        public void SyncPlaylistTrackSetup()
        {
            GlobalHelper = new GlobalHelper();
            Source = new Connection("localhost", 3306, "SourceChinook", "root", "");
            Destination = new Connection("localhost", 3306, "DestinationChinook", "root", "");
            Sync = new Synchronization();

            GlobalHelper.Setup<PlaylistTrack>(Sync);
            GlobalHelper.GetData<PlaylistTrack>(Sync, Source, Destination);
        }

        [Benchmark]
        public void SyncPlaylistTrack()
        {
            GlobalHelper.Compare<PlaylistTrack>(Sync);
            //GlobalHelper.GetSqlQueryForSyncData<PlaylistTrack>(Sync);

        }

        [GlobalCleanup(Target = nameof(SyncPlaylistTrack))]
        public void SyncPlaylistTrackCleanup()
        {
            GlobalHelper.CleanUp<PlaylistTrack>();
        }

        #endregion

        #region Sync PlaylistTrack


        [GlobalSetup(Target = nameof(SyncTrack))]
        public void SyncTrackSetup()
        {
            GlobalHelper = new GlobalHelper();
            Source = new Connection("localhost", 3306, "SourceChinook", "root", "");
            Destination = new Connection("localhost", 3306, "DestinationChinook", "root", "");
            Sync = new Synchronization();

            GlobalHelper.Setup<Track>(Sync);
            GlobalHelper.GetData<Track>(Sync, Source, Destination);
        }

        [Benchmark]
        public void SyncTrack()
        {
            GlobalHelper.Compare<Track>(Sync);
            //GlobalHelper.GetSqlQueryForSyncData<Track>(Sync);

        }

        [GlobalCleanup(Target = nameof(SyncTrack))]
        public void SyncTrackCleanup()
        {
            GlobalHelper.CleanUp<Track>();
        }

        #endregion
    }
}
