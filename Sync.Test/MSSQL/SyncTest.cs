using Sync.DB.Interface;
using Sync.MSSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sync.Test.MSSQL
{
    [TestClass]
    public class SyncTest
    {

        public IDatabase Source { get; set; }
        public IDatabase Destination { get; set; }

        [TestInitialize]
        public void Initialize() 
        {
            Source = new Connection("(localdb)\\MSSQLLocalDB", "SourceChinook", true);
            Destination = new Connection("(localdb)\\MSSQLLocalDB", "DestinationChinook", true);
        }

        [TestMethod]
        public void ConnectionTest()
        {
            var sourceTest = Source.TestConnection();

            Assert.IsTrue(sourceTest);

            var destinationTest = Destination.TestConnection();

            Assert.IsTrue(destinationTest);

            if(sourceTest == true && destinationTest == true)
                Console.WriteLine($"Connection Test is Successful");
            else
                Console.WriteLine($"Connection Test is not Successful");
        }

        [TestMethod]
        public void DatabaseTest()
        {

        }

        [TestMethod]
        public void QueryTest()
        {

        }

    }
}
