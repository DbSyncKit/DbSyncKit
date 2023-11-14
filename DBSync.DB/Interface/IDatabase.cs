namespace DBSync.DB.Interface
{
    public interface IDatabase
    {
        string GetConnectionString();
        List<T> ExecuteQuery<T>(string query, string tableName);
    }
}
