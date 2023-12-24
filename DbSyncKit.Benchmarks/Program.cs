using BenchmarkDotNet.Running;

namespace DbSyncKit.Benchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var b = new MySqlBenchmark();
            //b.MSSQLBenchmarkSetup();
            //b.ConnectionTest();
            //b.SyncAlbum();
            //b.SyncArtist();
            //b.SyncInvoiceLine();
            //b.SyncInvoice();
            //b.SyncCustomer();
            //b.SyncEmployee();
            //b.SyncGenre();
            //b.SyncPlaylist();
            //b.SyncPlaylistTrack();

            BenchmarkRunner.Run<MySqlBenchmark>();
        }
    }
}
