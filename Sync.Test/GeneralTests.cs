using Sync.DB.Interface;
using Sync.DB.Utils;
using System.Reflection;

namespace Sync.Test
{
    [TestClass]
    public class GeneralTests
    {
        [TestInitialize]
        public void Init()
        {
            //IF the Assembly is not loaded please make sure its loaded at startup.
            Assembly.Load("Sync.MSSQL");
            Assembly.Load("Sync.SQL");
            Assembly.Load("Sync.SQLite");
            Assembly.Load("Sync.Test.SampleContract");
        }


        [TestMethod]
        public void DataContractSearchTest()
        {
            Dictionary<string, string> contracts = new();
            // Get all loaded assemblies
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Iterate through each assembly
            foreach (var assembly in assemblies)
            {
                // Get all types in the assembly
                var types = assembly.GetTypes();

                // Filter types that inherit from DataContractUtility<T>
                var dataContractTypes = types
                    .Where(type =>
                        type.IsClass &&
                        !type.IsAbstract &&
                        type.BaseType != null &&
                        (type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(DataContractUtility<>) ||
                        type.GetInterfaces().Any(i => i == typeof(IDataContractComparer)))
                    );

                // Add the found types to the dictionary
                if( dataContractTypes.Any() )
                    Console.WriteLine($"Found {dataContractTypes.Count()} classes in {assembly.GetName().Name}");

                foreach (var dataContractType in dataContractTypes)
                {
                    contracts.Add(dataContractType.Name, dataContractType.Namespace);
                }

            }
            Console.WriteLine($"Found Total of {contracts.Count()} classes");
        }
    }
}