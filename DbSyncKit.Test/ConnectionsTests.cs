using DbSyncKit.DB.Interface;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbSyncKit.UnitTest
{
    public class ConnectionsTests
    {
        [Test]
        public void TestSQLiteConnection()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var Source = new SQLite.Connection(Path.Combine(baseDirectory, "Data", "sourceChinook.sqlite"));
            var Destination = new SQLite.Connection(Path.Combine(baseDirectory, "Data", "destinationChinook.sqlite"));
            ConnectionTest(Source, Destination);

        }

        [Test]
        public void TestMySQLConnection()
        {
            var Source = new MySQL.Connection("localhost", 3306, "SourceChinook", "root", "");
            var Destination = new MySQL.Connection("localhost", 3306, "DestinationChinook", "root", "");
            ConnectionTest(Source, Destination);

        }

        [Test]
        public void TestMSSQLConnection()
        {
            var Source = new MSSQL.Connection("(localdb)\\MSSQLLocalDB", false, "SourceChinook");
            var Destination = new MSSQL.Connection("(localdb)\\MSSQLLocalDB", false, "DestinationChinook");
            ConnectionTest(Source, Destination);

        }

        [Test]
        public void TestPostgreSQLConnection()
        {
            var Source = new PostgreSQL.Connection("localhost", 5432, "sourceChinook", "postgres", "");
            var Destination = new PostgreSQL.Connection("localhost", 5432, "destinationChinook", "postgres", "");

            ConnectionTest(Source, Destination);
        }

        public void ConnectionTest(IDatabase Source, IDatabase Destination)
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

    }
}
