using Sync.DB.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sync.DB
{
    public class DatabaseManager<T> where T : IDatabase
    {
        private readonly T _databaseProvider;

        public DatabaseManager(T databaseProvider)
        {
            _databaseProvider = databaseProvider ?? throw new ArgumentNullException(nameof(databaseProvider));
        }

        public List<TItem> ExecuteQuery<TItem>(string query, string tableName)
        {
            List<TItem> result = new List<TItem>();

            var dataset = _databaseProvider.ExecuteQuery(query, tableName);

            if (dataset.Tables.Contains(tableName))
            {
                foreach (DataRow row in dataset.Tables[tableName]!.Rows)
                {
                    result.Add((TItem)Activator.CreateInstance(typeof(TItem), new object[] { row })!);
                }
            }

            return result;
        }
    }
}
