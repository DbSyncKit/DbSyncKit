using DbSyncKit.DB.Helper;
using DbSyncKit.DB.Interface;
using DbSyncKit.Test.SampleContract.DataContract;
using System.Reflection;

namespace DbSyncKit.Test
{
    [TestClass]
    public class GeneralTests
    {
        [TestInitialize]
        public void Init()
        {
            //IF the Assembly is not loaded please make sure its loaded at startup.
            var listOfAssm = new List<string>();
            listOfAssm.Add("DbSyncKit.MSSQL");
            listOfAssm.Add("DbSyncKit.MySql");
            listOfAssm.Add("DbSyncKit.Postgre");
            listOfAssm.Add("DbSyncKit.Test.SampleContract");

            foreach (var assm in listOfAssm)
            {
                try
                {
                    Assembly.Load(assm);
                }
                catch (Exception) { }
            }
        }

        [TestMethod]
        public void CheckAttributes()
        {
            var queryHelper = new QueryHelper();
            var TableName = queryHelper.GetTableName<Album>();
            var SchemaName = queryHelper.GetTableSchema<Album>();
            var KeyAttribues = queryHelper.GetKeyColumns<Album>();
            var ExcludeAttribues = queryHelper.GetExcludedColumns<Album>();
            var withID = queryHelper.GetInsertWithID<Album>();

            Console.WriteLine($"TableName: {TableName}, SchemaName: {SchemaName}");
            Console.WriteLine("KeyAttributes: " + string.Join(", ", KeyAttribues));
            Console.WriteLine("ExcludeAttribues: " + string.Join(", ", ExcludeAttribues));
            Console.WriteLine("GenerateInsertwithID: " + withID);
        }
    }
}