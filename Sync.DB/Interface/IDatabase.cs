using Sync.DB.Enum;

namespace Sync.DB.Interface
{
    public interface IDatabase
    {
        DatabaseProvider Provider { get; }
        string GetConnectionString();
        List<T> ExecuteQuery<T>(string query, string tableName);
    }
}
