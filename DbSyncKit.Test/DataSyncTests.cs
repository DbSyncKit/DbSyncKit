using DbSyncKit.DB.Interface;
using DbSyncKit.SQLite;
using DbSyncKit.UnitTest.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbSyncKit.UnitTest
{
    public class DataSyncTests : DataSyncTestBase
    {

        protected override IDatabase GetSourceDatabase()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return new Connection(Path.Combine(baseDirectory, "Data", "sourceChinook.sqlite"));
        }

        protected override IDatabase GetDestinationDatabase()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return new Connection(Path.Combine(baseDirectory, "Data", "destinationChinook.sqlite"));
        }
    }
}
