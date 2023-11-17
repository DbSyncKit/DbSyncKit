using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sync.DB.Interface
{
    public interface IQuerryGenerator
    {
        string GenerateSelectQuery(string tableName, List<string> ListOfColumns,string schemaName);

        string GenerateUpdateQuery<T>(T DataContract, List<string> keyColumns, List<string> excludedColumns, Dictionary<string, object> editedProperties);
    }
}
