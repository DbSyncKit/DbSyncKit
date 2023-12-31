using BenchmarkDotNet.Running;

namespace DbSyncKit.Benchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var b = new SyncCompareDifferenceBenchmark();
            //b.MSSQLBenchmarkSetup();
            ////b.ConnectionTest();
            //b.SyncAlbumSetup();
            //b.SyncAlbum();
            //b.SyncAlbumCleanup();
            //b.SyncArtist();
            //b.SyncInvoiceLine();
            //b.SyncInvoice();
            //b.SyncCustomer();
            //b.SyncEmployee();
            //b.SyncGenre();
            //b.SyncPlaylist();
            //b.SyncPlaylistTrack();
            //b.SyncTrackSetup();
            //b.SyncTrack();
            //b.SyncTrackCleanup();

            BenchmarkRunner.Run<SyncCompareDifferenceBenchmark>();
        }
    }
}
