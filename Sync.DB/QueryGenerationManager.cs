using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sync.DB.Interface;

namespace Sync.DB
{
    public class QueryGenerationManager
    {
        private readonly IQuerryGenerator _querryGenerator;

        public QueryGenerationManager(IQuerryGenerator querryGenerator)
        {
            _querryGenerator = querryGenerator;
        }

        #region Public Methods
        public string GenerateSelectQuery(string tableName, List<string> listOfColumns, string schemaName)
        { 
            return _querryGenerator.GenerateSelectQuery(tableName, listOfColumns, schemaName);
        }

        public string GenerateUpdateQuery<T>(T DataContract, List<string> keyColumns, List<string> excludedColumns, Dictionary<string, object> editedProperties) where T : IDataContractComparer
        {
            return _querryGenerator.GenerateUpdateQuery<T>(DataContract,keyColumns,excludedColumns, editedProperties);
        }

        #endregion
    }
}
