using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sync.DB.Interface
{
    public interface IQueryGenerator
    {
        string GenerateSelectQuery(string tableName, List<string> ListOfColumns,string schemaName);

        string GenerateUpdateQuery<T>(T DataContract, List<string> keyColumns, List<string> excludedColumns, Dictionary<string, object> editedProperties) where T : IDataContractComparer;

        string GenerateDeleteQuery<T>(T entity, List<string> keyColumns) where T : IDataContractComparer;

        string GenerateInsertQuery<T>(T entity, List<string> keyColumns, List<string> excludedColumns) where T : IDataContractComparer;

        string GenerateComment(string comment);
    }
}
