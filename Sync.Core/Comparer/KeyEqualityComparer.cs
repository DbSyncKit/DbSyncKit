using System.ComponentModel.DataAnnotations;
using System.Reflection;

using Sync.DB.Interface;
using Sync.DB.Utils;

namespace Sync.Core.Comparer
{
    public class KeyEqualityComparer<T> : IEqualityComparer<T> where T : IDataContractComparer
    {
        private readonly PropertyInfo keyProperty;

        private readonly List<string> keyColumns;
        private readonly List<string> ignoredColumns;

        public KeyEqualityComparer(List<string> keyColumns, List<string> ignoredColumns)
        {
            keyProperty = typeof(T).GetProperties()
                .FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(KeyAttribute)))!;

            if (keyProperty == null)
            {
                throw new InvalidOperationException($"No property with KeyAttribute found in type {typeof(T).Name}");
            }
            if (!keyColumns.Contains(keyProperty.Name))
                keyColumns.Add(keyProperty.Name);

            this.keyColumns = keyColumns;
            this.ignoredColumns = ignoredColumns;
        }


        public bool Equals(T x, T y)
        {
            var properties = typeof(T).GetProperties()
                .Where(prop => keyColumns.Contains(prop.Name) && !ignoredColumns.Contains(prop.Name));

            return properties.All(prop => Equals(prop.GetValue(x), prop.GetValue(y)));
        }

        public int GetHashCode(T obj)
        {
            unchecked
            {
                int hash = 17;
                var properties = typeof(T).GetProperties()
                    .Where(prop => keyColumns.Contains(prop.Name) && !ignoredColumns.Contains(prop.Name));

                foreach (var prop in properties)
                {
                    var value = prop.GetValue(obj);
                    hash = hash ^ ((value?.GetHashCode() ?? 0) + 23);
                }

                return hash;
            }
        }
    }
}
